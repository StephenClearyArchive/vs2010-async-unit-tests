To completely uninstall Async Unit Tests from Visual Studio, you'll need to first download [this script](../uninstall.ps1) to your hard drive.

Then run it from within Visual Studio's Package Manager Console (you can run a script by typing ". " and then the path to the script, enclosed in quotes). The script will fail to delete the dll because Visual Studio is still using it.

Restart Visual Studio, and then run the same script again from within Visual Studio's Package Manager Console.

Delete the uninstall script, and Async Unit Tests will be completely uninstalled.