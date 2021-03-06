﻿using System;
using System.Linq;
using System.Threading;
using Platform;
using Platform.StreamClients;
using Platform.ViewClients;
using TestClient.Commands;

namespace TestClient
{
    public class Client
    {
        private static readonly ILogger Log = LogManager.GetLoggerFor<Client>();
        public readonly ClientOptions Options;
        readonly CommandProcessorCollection _commands = new CommandProcessorCollection(Log);
        
        public IRawEventStoreClient EventStores;
        public string ClientHttpBase;
        public ViewClient Views;

        public Client(ClientOptions options)
        {
            Options = options;
            // TODO : pass server options
            ClientHttpBase = string.Format("http://{0}:{1}", options.Ip, options.HttpPort);
            Views = PlatformClient.ConnectToViewStorage(options.StoreLocation, options.ViewsFolder);
            
            UseEventStore("seo");

            RegisterCommands();
        }

        public void UseEventStore(string storeId = "default")
        {
            EventStores = PlatformClient.ConnectToEventStore(Options.StoreLocation, storeId, ClientHttpBase);
        }

        void RegisterCommands()
        {
            //_commands.Register(new ExitProcessor());
            //_commands.Register(new WriteEventsFloodProcessor());
            //_commands.Register(new WriteBatchProcessor());
            //_commands.Register(new WriteBatchFloodProcessor());
            _commands.Register(new UsageProcessor(_commands));
            _commands.Register(new FindingUrlProcessor());
            //_commands.Register(new WriteProccessor());
            //_commands.Register(new EnumerateProcessor());
            //_commands.Register(new BasicTestProcessor());
            //_commands.Register(new BasicVerifyProcessor());
            //_commands.Register(new BasicBenchmarkProcessor());
            //_commands.Register(new SmartAppBenchmarkProcessor());
            
            //_commands.Register(new ShutdownProcessor());
            //_commands.Register(new StartLocalServerProcessor());

            //_commands.Register(new ResetStoreProcessor());
            
            //_commands.Register(new ReadProcessor());

            //_commands.Register(new ViewReadWriteFloodProcessor());
            //_commands.Register(new UsingProcessor());
            //_commands.Register(new EventPointerFloodProcessor());
        }

         

        public void Run()
        {
            if (Options.Command.Any())
            {
                var @join = string.Join(" ", Options.Command);
                if (!ExecuteLine(@join))
                {
                    Application.Exit(ExitCode.Error, "Error while processing " + @join);
                }
                return;
            }
            
            
            
            
            while (true)
            {
                Console.Write(">>> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Empty command. Type 'usage' if unsure.");
                }
                else
                {
                    ExecuteLine(line);
                    Thread.Sleep(100);
                }
            }
        }

       

        bool ExecuteLine(string line)
        {
            try
            {
                var args = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                //Log.Info("Processing command: {0}.", string.Join(" ", args));

                var context = new CommandProcessorContext(this, Log);
                return _commands.TryProcess(context, args);
            }
            catch (Exception exception)
            {
                //Log.ErrorException(exception, "Error while executing command");
                return false;
            }
        }
    }
}