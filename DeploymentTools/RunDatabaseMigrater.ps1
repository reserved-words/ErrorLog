param($MigraterPath, $ConnectionString, $DatabaseName, $AppUser)

Start-Process -FilePath $MigraterPath -ArgumentList ("`"" + $ConnectionString + "`" `"" + $DatabaseName + "`" `"" + $AppUser + "`"")