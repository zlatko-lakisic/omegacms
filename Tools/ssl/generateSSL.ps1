$i = 0
$path = (Get-Item -Path ".\").FullName;
Remove-Item crt -ErrorAction Ignore -Recurse
New-Item -Name "crt" -ItemType "directory"

$procTools = @"

using System;
using System.Diagnostics;

namespace Proc.Tools
{
  public static class exec
  {
    public static int runCommand(string executable, string args = "", string cwd = "", string verb = "runas") {

      //* Create your Process
      Process process = new Process();
      process.StartInfo.FileName = executable;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;

      //* Optional process configuration
      if (!String.IsNullOrEmpty(args)) { process.StartInfo.Arguments = args; }
      if (!String.IsNullOrEmpty(cwd)) { process.StartInfo.WorkingDirectory = cwd; }
      if (!String.IsNullOrEmpty(verb)) { process.StartInfo.Verb = verb; }

      //* Set your output and error (asynchronous) handlers
      process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
      process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

      //* Start process and handlers
      process.Start();
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();
      process.WaitForExit();

      //* Return the commands exit code
      return process.ExitCode;
    }
    public static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine) {
      //* Do your stuff with the output (write to console/log/StringBuilder)
      Console.WriteLine(outLine.Data);
    }
  }
}
"@

Add-Type -TypeDefinition $procTools -Language CSharp


function generateCSR{
    [CmdletBinding()]
    param($domain)
    $keyname = $domain.Replace("*.", "").Replace(".", "_")
    $command = "le.pl --key account.key --csr `"${path}\csr\$keyname.csr`" --csr-key `"${path}\domain_keys\$keyname.key`" --crt `"$keyname.crt`" --domains `"${domain}`" --handle-as dns --api 2 --live"
    
    $[Proc.Tools.exec]::runCommand("C:\Windows\SysWOW64\cmd.exe", "/c ${command}", $path)
}

# Set OMEGA_SSL_CERT_DOMAIN in your environment, or pass -domain (default is reserved example domain).
$certDomain = if ($env:OMEGA_SSL_CERT_DOMAIN) { $env:OMEGA_SSL_CERT_DOMAIN } else { "*.core.cms.example.com" }
generateCSR -domain $certDomain
pause