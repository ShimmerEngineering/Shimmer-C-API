// See https://aka.ms/new-console-template for more information
using ShimmerAPI;

Console.WriteLine("Hello, World!");

ShimmerSDLog sdLog = new ShimmerSDLog("C:\\Users\\JC\\Shimmer_Workspace\\Backup\\2025-07-04_15.41.07\\e68ecd1ded3c\\data\\DefaultTrial_1751614671\\Shimmer_ED3C-000\\000");
//ShimmerSDLog sdLog = new ShimmerSDLog("C:\\Users\\JC\\Shimmer_Workspace\\Backup\\2025-12-27_17.04.37\\e8eb1b9767ad\\data\\DefaultTrial_1766826152\\Shimmer_67AD-000\\000");
Console.WriteLine(sdLog.GetShimmerVersion());
Console.WriteLine(sdLog.GetShimmerAddress());
Logging log = new Logging("test.csv", ",");
while (!sdLog.EndOfFile)
{
    ObjectCluster ojc = sdLog.ReadPacketMsg();
    if (ojc!=null)
    log.WriteData(ojc);
    
}
log.CloseFile();