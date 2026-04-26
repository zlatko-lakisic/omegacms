$service = Get-WmiObject -Class Win32_Service -Filter "Name='Omega Async Task Processor'"
$service.delete()