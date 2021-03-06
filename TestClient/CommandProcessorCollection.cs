﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Platform;
using TestClient.Commands;

namespace TestClient
{
    public class CommandProcessorCollection
    {
        private readonly IDictionary<string, ICommandProcessor> _processors = new Dictionary<string, ICommandProcessor>();
        private readonly ILogger _log;

        public CommandProcessorCollection(ILogger log)
        {
            _log = log;
        }

        public IEnumerable<ICommandProcessor> RegisteredProcessors
        {
            get { return _processors.Values; }
        }

        public void Register(ICommandProcessor commandProcessor)
        {
            var cmd = commandProcessor.Key.ToUpper();

            if (_processors.ContainsKey(cmd))
                throw new Exception(string.Format("The processor for command '{0}' is already registered", cmd));

            _processors.Add(cmd, commandProcessor);
        }

        public bool TryProcess(CommandProcessorContext context, string[] args)
        {
            if (args == null || args.Length == 0)
            {
                _log.Error("Empty command");
                throw new Exception("Empty command");
            }

            var commandName = args[0].ToUpper();
            var commandArgs = args.Skip(1).ToArray();

            ICommandProcessor commandProcessor;
            if (!_processors.TryGetValue(commandName, out commandProcessor))
            {
                _log.Info("Unknown command: '{0}'", commandName);
                new UsageProcessor(this).Execute(context, CancellationToken.None, new string[0]);
                return false;
            }

            var result = false;

            var timeout = context.Client.Options.Timeout;

            using (var source = new CancellationTokenSource())
            {
                try
                {
                    var token = source.Token;
                    if (timeout > 0)
                    {
                        result = new WaitFor<bool>(TimeSpan.FromSeconds(timeout)).Run(
                                () => commandProcessor.Execute(context, token, commandArgs));
                    }
                    else
                    {
                        result = commandProcessor.Execute(context, token,commandArgs);
                    }
                }
                catch (TimeoutException ex)
                {
                    _log.Error("Command didn't finish in {0} seconds", timeout);
                }
                catch (Exception exc)
                {
                    _log.ErrorException(exc, "Failure while processing {0}", commandName);
                }
                finally
                {
                    source.Cancel();
                }
            }
            return result;
        }
    }
}