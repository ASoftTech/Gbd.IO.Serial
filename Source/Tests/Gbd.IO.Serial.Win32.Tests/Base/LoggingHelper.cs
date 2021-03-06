﻿using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Formatting.Display;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.Win32.Tests.Base {
    internal static class LoggingHelper {
        private static readonly Subject<LogEvent> s_logEventSubject = new Subject<LogEvent>();
        private const string CaptureCorrelationIdKey = "CaptureCorrelationId";

        private static readonly MessageTemplateTextFormatter s_formatter = new MessageTemplateTextFormatter(
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", null);

        static LoggingHelper() {
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .Observers(observable => observable.Subscribe(logEvent => s_logEventSubject.OnNext(logEvent)))
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        public static IDisposable Capture(ITestOutputHelper testOutputHelper) {
            var captureId = Guid.NewGuid();

            Func<LogEvent, bool> filter = logEvent =>
                logEvent.Properties.ContainsKey(CaptureCorrelationIdKey) &&
                logEvent.Properties[CaptureCorrelationIdKey].ToString() == captureId.ToString();

            var subscription = s_logEventSubject.Where(filter).Subscribe(logEvent => {
                using (var writer = new StringWriter()) {
                    s_formatter.Format(logEvent, writer);
                    testOutputHelper.WriteLine(writer.ToString());
                }
            });
            var pushProperty = LogContext.PushProperty(CaptureCorrelationIdKey, captureId);

            return new DisposableAction(() => {
                subscription.Dispose();
                pushProperty.Dispose();
            });
        }

        private class DisposableAction : IDisposable {
            private readonly Action _action;

            public DisposableAction(Action action) {
                _action = action;
            }

            public void Dispose() {
                _action();
            }
        }
    }
}