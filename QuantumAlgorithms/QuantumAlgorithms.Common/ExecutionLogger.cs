﻿using System;
using QuantumAlgorithms.DataService;
using QuantumAlgorithms.Domain;

namespace QuantumAlgorithms.Common
{
    public interface IExecutionLogger : ILogger
    {
        Guid GetExecutionId();
        void SetExecutionId(Guid executionId);
    }

    public class ExecutionLogger : IExecutionLogger
    {
        private readonly IDataService<ExecutionMessage> _dataService;
        private Guid _executionId;

        public ExecutionLogger(IDataService<ExecutionMessage> dataService)
        {
            _dataService = dataService;
        }

        public void Info(string message) =>
            Log(message, ExecutionMessageSeverity.Info);

        public void Warning(string message)
        {
            var dataService = _dataService as ExecutionMessageDataService;
            var lastMessage = dataService.GetLastByExecutionId(_executionId);

            if (lastMessage == null || lastMessage.Severity != ExecutionMessageSeverity.Error)
                Log(message, ExecutionMessageSeverity.Warning);
        }

        public void Error(string message) =>
            Log(message, ExecutionMessageSeverity.Error);

        public Guid GetExecutionId() =>
            _executionId;

        public void SetExecutionId(Guid executionId) =>
            _executionId = executionId;


        private void Log(string message, ExecutionMessageSeverity severity)
        {
            _dataService.Create(new ExecutionMessage
            {
                TimeStamp = DateTime.Now,
                Message = message,
                Severity = severity,
                QuantumAlgorithmId = _executionId
            });
            _dataService.SaveChanges();
        }
    }
}
