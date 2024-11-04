/*
// CommandLines.cs / pmcenter project / https://github.com/Elepover/pmcenter
// Commandlines are processed here.
// Copyright (C) The pmcenter authors. Licensed under the Apache License (Version 2.0).
*/

using System.Threading.Tasks;
using pmcenter.CommandLines;

namespace pmcenter
{
    public static class CmdLineProcess
    {
        private static readonly CommandLineRouter cmdLineRouter = new();

        static CmdLineProcess()
        {
            cmdLineRouter.RegisterCommand(new HelpCmdLine());
            cmdLineRouter.RegisterCommand(new InfoCmdLine());
            cmdLineRouter.RegisterCommand(new NonServiceModeCmdLine());
            cmdLineRouter.RegisterCommand(new SetupWizardCmdLine());
            cmdLineRouter.RegisterCommand(new ResetCmdLine());
            cmdLineRouter.RegisterCommand(new BackupCmdLine());
            cmdLineRouter.RegisterCommand(new UpdateCmdLine());
        }

        public static async Task RunCommand(string CommandLine)
        {
            _ = await cmdLineRouter.Execute(CommandLine).ConfigureAwait(false);
        }
    }
}
