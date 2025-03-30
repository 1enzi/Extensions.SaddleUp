using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VitoExtensions.SaddleUp.ToolWindows;

namespace VitoExtensions.SaddleUp
{
    internal sealed class SaddleUpCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new("5f5e1d0d-9e67-4d14-9e67-abcde1234567");

        private readonly AsyncPackage _package;

        private SaddleUpCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            var commandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, commandId);
            commandService?.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            if (await package.GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
                _ = new SaddleUpCommand(package, commandService);
        }

        private async void Execute(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (Package.GetGlobalService(typeof(DTE)) is not DTE2 dte)
                return;

            var active = dte.ActiveDocument?.FullName;

            foreach (Window window in dte.Windows)
            {
                try
                {
                    var doc = window.Document;
                    if (doc == null || doc.FullName == active)
                        continue;

                    var frame = GetWindowFrameFromMoniker(doc.FullName);
                    if (frame != null && TryGetIsPinned(frame, out var isPinned) && !isPinned)
                        window.Close(vsSaveChanges.vsSaveChangesPrompt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SaddleUp error: {ex.Message}");
                }
            }

            await ShowSaddleUpWindowAsync();
        }

        private IVsWindowFrame GetWindowFrameFromMoniker(string moniker)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var shell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
            shell.GetDocumentWindowEnum(out var enumFrames);

            var frame = new IVsWindowFrame[1];
            while (enumFrames.Next(1, frame, out var fetched) == 0 && fetched == 1)
            {
                frame[0].GetProperty((int)__VSFPROPID.VSFPROPID_pszMkDocument, out var monikerObj);
                if (monikerObj is string docMoniker &&
                    string.Equals(docMoniker, moniker, StringComparison.OrdinalIgnoreCase))
                    return frame[0];
            }

            return null;
        }

        private static bool TryGetIsPinned(IVsWindowFrame frame, out bool isPinned)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            isPinned = false;

            try
            {
                var hr = frame.GetProperty((int)__VSFPROPID5.VSFPROPID_IsPinned, out var result);
                if (hr == 0 && result is bool b)
                {
                    isPinned = b;
                    return true;
                }
            }
            catch
            {
                // Нежный COM-провал
            }

            return false;
        }

        private async Task ShowSaddleUpWindowAsync()
        {
            await _package.JoinableTaskFactory.SwitchToMainThreadAsync();

            var window = await _package.ShowToolWindowAsync(
                typeof(SaddleUpSplash),
                0,
                true,
                _package.DisposalToken);

            if (window is null || window.Frame is null)
                throw new NotSupportedException("Can't show 'Saddle Up! 🐎' window!");

            if (window.Content is SaddleUpSplashControl control)
            {
                control.RefreshQuote();
            }
        }
    }
}
