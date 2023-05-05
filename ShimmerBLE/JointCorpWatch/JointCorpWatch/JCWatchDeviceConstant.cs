using System;
using System.Collections.Generic;
using System.Text;

namespace JointCorpWatch
{
    public class JCWatchDeviceConstant
    {
        public const byte CMD_SET_TIME = (byte)0x01;
        public const byte CMD_GET_TIME = (byte)0x41;
        public const byte CMD_Set_UseInfo = (byte)0x02;
        public const byte CMD_GET_USERINFO = (byte)0x42;
        public const byte CMD_Set_DeviceInfo = (byte)0x03;
        public const byte CMD_Get_DeviceInfo = (byte)0x04;
        public const byte CMD_Set_DeviceID = (byte)0x05;
        public const byte CMD_Enable_Activity = (byte)0x09;
        public const byte CMD_Set_Goal = (byte)0x0b;
        public const byte CMD_Get_Goal = (byte)0x4b;
        public const byte CMD_Get_BatteryLevel = (byte)0x13;
        public const byte CMD_Get_Address = (byte)0x22;
        public const byte CMD_Get_Version = (byte)0x27;
        public const byte CMD_Reset = (byte)0x12;
        public const byte CMD_Mcu_Reset = (byte)0x2e;
        public const byte CMD_Set_MOT_SIGN = (byte)0x36;
        public const byte CMD_Set_Name = (byte)0x3d;
        public const byte CMD_Get_Name = (byte)0x3e;
        public const byte CMD_Set_Auto = (byte)0x2a;
        public const byte CMD_Get_Auto = (byte)0x2b;
        public const byte CMD_Set_Clock = (byte)0x23;
        public const byte CMD_Notify = (byte)0x4d;
        public const byte CMD_Set_ActivityAlarm = (byte)0x25;
        public const byte CMD_Get_ActivityAlarm = (byte)0x26;
        public const byte CMD_Get_TotalData = (byte)0x51;
        public const byte CMD_Get_DetailData = (byte)0x52;
        public const byte CMD_Get_SleepData = (byte)0x53;
        public const byte CMD_Get_HeartData = (byte)0x54;
        public const byte CMD_Get_Blood_oxygen = (byte)0x60;
        public const byte CMD_Get_OnceHeartData = (byte)0x55;
        public const byte CMD_Get_Clock = (byte)0x57;
        public const byte CMD_Get_SPORTData = (byte)0x5C;
        public const byte CMD_Get_HrvTestData = (byte)0x56;
        public const byte CMD_SET_SOCIAL = (byte)0x64;
        public const byte CMD_Start_EXERCISE = (byte)0x19;
        public const byte CMD_HeartPackageFromDevice = (byte)0x18;//
        public const byte CMD_Start_Ota = (byte)0x47;
        public const byte CMD_Set_TemperatureCorrection = (byte)0x38;
        public const byte CMD_Set_HeartbeatPackets = (byte)0x06;
        public const byte Enter_photo_mode = (byte)0x20;
        public const byte Enter_photo_modeback = (byte)0x16;
        public const byte Exit_photo_mode = (byte)0x10;
        public const byte Openecg = (byte)0x99;
        public const byte Closeecg = (byte)0x98;
        public const byte CMD_ECGQuality = (byte)0x83;
        public const byte GetEcgPpgStatus = (byte)0x9c;
        public const byte CMD_ECGDATA = (byte)0xAA;
        public const byte CMD_PPGDATA = (byte)0xAB;
        public const byte Weather = (byte)0x15;
        public const byte Braceletdial = (byte)0x24;
        public const byte SportMode = (byte)0x14;
        public const byte GetSportMode = (byte)0x44;
        public const byte CMD_Get_WorkOutReminder = (byte)0x29;
        public const byte ecgandppg = (byte)0x9E;
        /* public const byte ReadSerialNumber= (byte)0x62;*/
        public const byte LanguageSwitching = (byte)0x29;
        public const byte CMD_heart_package = (byte)0x17;
        public const byte GPSControlCommand = (byte)0x34;
        public const byte CMD_Get_GPSDATA = (byte)0x5A;
        public const byte Clear_Bracelet_data = (byte)0x61;
        public const byte Sos = (byte)0xFE;
        public const byte Temperature_history = (byte)0x62;
        public const byte GetAxillaryTemperatureDataWithMode = (byte)0x65;
        public const byte Get3D = (byte)0x49;
        public const byte PPG = (byte)0x39;
        public const byte GetECGwaveform = (byte)0x71;
        public const byte GetAutomaticSpo2Monitoring = (byte)0x66;
        public const byte spo2 = (byte)0x29;
        public const byte MeasurementWithType = (byte)0x28;
        public const byte Qrcode = (byte)0xB0;
        public const byte openRRIntervalTime = (byte)0x9B;
        public const byte openPPG = (byte)0x3a;
        public const byte ppgWithMode = (byte)0x78;
    }

    public class JCWatchDeviceKey
    {
        public const String heartValue = "heartValue";
        public const String hrvValue = "hrvValue";
        public const String Quality = "Quality";

        public const String HangUp = "HangUp";
        public const String Telephone = "Telephone";
        public const String Photograph = "Photograph";
        public const String CanclePhotograph = "CanclePhotograph";
        public const String type = "type";
        public const String Play = "Play";
        public const String Suspend = "Suspend";
        public const String LastSong = "LastSong";
        public const String NextSong = "NextSong";
        public const String VolumeReduction = "VolumeReduction";
        public const String VolumeUp = "VolumeUp";
        public const String FindYourPhone = "FindYourPhone";
        public const String Cancle_FindPhone = "Cancle_FindPhone";
        public const String SOS = "SOS";
        public const String DataType = "dataType";
        public const String enterActivityModeSuccess = "enterActivityModeSuccess";
        public const String Type = "Type";
        public const String RRIntervalData = "RRIntervalData";
        public const String Manual = "Manual";
        public const String automatic = "automatic";
        public const String Data = "dicData";
        public const String End = "dataEnd";
        public const String index = "index";
        public const String scanInterval = "scanInterval";
        public const String scanTime = "scanTime";
        public const String signalStrength = "signalStrength";
        public const String arrayX = "arrayX";
        public const String arrayY = "arrayY";
        public const String arrayZ = "arrayZ";
        public const String arrayPpgRawData = "arrayPpgRawData";
        public const String KGpsResCheck0 = "KGpsResCheck0"; // 设备时间   GET_DEVICE_Time
        public const String KGpsResCheck1 = "KGpsResCheck1"; // 设备时间
        public const String Band = "Band";
        public const String KFinishFlag = "finish";
        public const String DeviceTime = "strDeviceTime"; // 设备时间   GET_DEVICE_Time
        public const String GPSTime = "gpsExpirationTime"; // 设备时间   GET_DEVICE_Time
        public const String TimeZone = "TimeZone"; // 设备时间   GET_DEVICE_Time

        /*
         *  GET_PERSONAL_INFO
         *   sex         性别
         *   Age         年龄
         *   Height      身高
         *   Weight      体重
         *   stepLength  步长
         *   deviceId    设备ID 
         */
        public const String Gender = "MyGender";
        public const String Age = "MyAge";
        public const String Height = "MyHeight";
        public const String Weight = "MyWeight";
        public const String Stride = "MyStride";
        public const String KUserDeviceId = "deviceId";


        /*
         *  GET_DEVICE_INFO
         *  distanceUnit  距离单位
         *  hourState     12小时24小时显示
         *  handleEnable  抬手检查使能标志
         *  handleSign    抬手检测左右手标志
         *  screenState   横竖屏显示
         *  anceEnable    ANCS使能开关
         */
        public const String DistanceUnit = "distancUnit";
        public const String TimeUnit = "timeUnit";
        public const String TempUnit = "temperatureUnit";
        public const String WristOn = "wristOn";
        public const String TemperatureUnit = "TemperatureUnit";
        public const String NightMode = "NightMode";
        public const String LeftOrRight = "handleSign";
        //public const String ScreenShow = "screenState";
        public const String Dialinterface = "dialinterface";
        public const String SocialDistancedwitch = "SocialDistancedwitch";
        public const String ChineseOrEnglish= "ChineseOrEnglish";
        public const String ScreenBrightness = "dcreenBrightness";
        public const String KBaseHeart = "baseHeartRate";
        public const String isHorizontalScreen = "isHorizontalScreen";

        /*
         *  SET_STEP_MODEL
         *totalSteps   总步数
         *calories     卡路里
         *distance     距离
         *time         时间
         *heartValue   心率值
         */

        public const String Step = "step";
        public const String Calories = "calories";
        public const String Distance = "distance";
        public const String ExerciseMinutes = "exerciseMinutes";
        public const String HeartRate = "heartRate";
        public const String ActiveMinutes = "ExerciseTime";
        public const String TempData = "TempData";
        public const String StepGoal = "stepGoal";   // 目标步数值  GET_GOAL
        public const String BatteryLevel = "batteryLevel";  // 电量级别    READ_DEVICE_BATTERY
        public const String MacAddress = "macAddress"; // MAC地址    READ_MAC_ADDRESS
        public const String DeviceVersion = "deviceVersion";  // 版本号     READ_VERSION
        public const String DeviceName = "deviceName";  // 设备名称    GET_DEVICE_NAME
        public const String TemperatureCorrectionValue = "TemperatureCorrectionValue";  // 设备名称    GET_DEVICE_NAME

        /*
         *  GET_AUTOMIC_HEART
         *workModel         工作模式
         *heartStartHour    开始运动时间的小时
         *heartStartMinter  开始运动时间的分钟
         *heartEndHour      结束运动时间的小时
         *heartEndMinter      结束运动时间的分钟
         *heartWeek         星期使能
         *workTime          工作模式时间
         */
        public const String WorkMode = "workModel";
        public const String StartTime = "heartStartHour";
        public const String KHeartStartMinter = "heartStartMinter";
        public const String EndTime = "heartEndHour";
        public const String KHeartEndMinter = "heartEndMinter";
        public const String Weeks = "weekValue";
        public const String IntervalTime = "intervalTime";


        /*
         *  READ_SPORT_PERIOD
         *StartTimeHour       开始运动时间的小时
         *StartTimeMin     开始运动时间的分钟
         *EndTimeHour         结束运动时间的小时
         *EndTimeMin       结束运动时间的分钟
         *Week      星期使能
         *KSportNotifierTime    运动提醒周期
         */
        public const String StartTimeHour = "sportStartHour";
        public const String StartTimeMin = "sportStartMinter";
        public const String EndTimeHour = "sportEndHour";
        public const String EndTimeMin = "sportEndMinter";

        public const String LeastSteps = "leastSteps";

        /*
         *  GET_STEP_DATA
         *historyDate       日期：年月日
         *historySteps      步数
         *historyTime       运动时间
         *historyDistance   距离
         *Calories  卡路里
         *historyGoal       目标
         */
        public const String Date = "date";
        public const String Size = "size";
        public const String Goal = "goal";



        /*
         *  GET_STEP_DETAIL
         *Date       日期：年月日时分秒
         *ArraySteps          步数
         *Calories       卡路里
         *Distance       距离
         *KDetailMinterStep     10分钟内每一分钟的步数
         */

        public const String ArraySteps = "arraySteps";

        public const String KDetailMinterStep = "detailMinterStep";
        public const String temperature = "temperature";
        public const String axillaryTemperature = "axillaryTemperature";

        /*
         * GET_SLEEP_DETAIL
         *Date        日期：年月日时分秒
         *KSleepLength      睡眠数据的长度
         *ArraySleep    5分钟的睡眠质量 (总共24个数据，每一个数据代表五分钟)
         */

        public const String KSleepLength = "sleepLength";
        public const String ArraySleep = "arraySleepQuality";
        public const String sleepUnitLength = "sleepUnitLength";//是不是一分钟的睡眠数据 1为1分钟数据 0为5分钟数据

        /*
         *  GET_HEART_DATA
         *Date        日期：年月日时分秒
         *ArrayDynamicHR        10秒一个心率值，总共12个心率值
         */

        public const String ArrayDynamicHR = "arrayDynamicHR";
        public const String Blood_oxygen = "Blood_oxygen";



        /*
         * GET_ONCE_HEARTDATA
         *Date        日期：年月日时分秒
         *StaticHR       心率值
         */

        public const String StaticHR = "onceHeartValue";

        /*
         *  GET_HRV_DATA
         *Date          日期：年月日时分秒
         *HRV         HRV值
         *VascularAging    血管老化度值
         *HeartRate    心率值
         *Stress         疲劳度
         */

        public const String HRV = "hrv";
        public const String VascularAging = "vascularAging";
        public const String Fatiguedegree = "fatigueDegree";

        public const String Stress = "stress";
        public const String HighPressure = "highPressure";
        public const String LowPressure = "lowPressure";
        public const String highBP = "highBP";
        public const String lowBP = "lowBP";

        /*
         *GET_ALARM
         *KAlarmId          0到4闹钟编号
         *ClockType        闹钟类型
         *ClockTime        闹钟时间的小时
         *KAlarmMinter      闹钟时间的分钟
         *Week  星期使能
         *KAlarmLength      长度
         *KAlarmContent     文本的内容
         */
        public const String KAlarmId = "alarmId";
        public const String OpenOrClose = "clockOpenOrClose";
        public const String ClockType = "clockType";
        public const String ClockTime = "alarmHour";
        public const String KAlarmMinter = "alarmMinter";
        public const String Week = "weekValue";
        public const String KAlarmLength = "alarmLength";
        public const String KAlarmContent = "dicClock";


        /***********************GET_HRV_TESTDATA***************************************************/
        /*
         *KBloodTestLength      数据长度
         *KBloodTestProgress    进度
         *KBloodTestValue       本次PPG获得的值
         *KBolldTestCurve       本次波型的高度
         */
        public const String KBloodTestLength = "bloodTestLength";
        public const String KBloodTestProgress = "bloodTestProgress";
        public const String KBloodTestValue = "bloodTestValue";
        public const String KBloodTestCurve = "bloodTestCurve";

        /*
         *KBloodResultPercent       反弹的百分比
         *KBloodResultRebound       平均反弹高度
         *KBloodResultMax           最大高度
         *KBloodResultRank          结果级别（1到6）
         */
        public const String KBloodResultPercent = "bloodPercent";
        public const String KBloodResultRebound = "bloodRebound";
        public const String KBloodResultMax = "bloodResultMax";
        public const String KBloodResultRank = "bloodResultRank";


        /*
         *KHrvTestProgress  进度
         *KHrvTestWidth     本次心跳的宽度
         *KHrvTestValue     心率值
         */
        public const String KHrvTestProgress = "hrvTestProgress";
        public const String KHrvTestWidth = "hrvTestWidth";
        public const String KHrvTestValue = "hrvTestValue";

        /*
         *KHrvResultState   SDNN结果  如果是0,说明检测失败
         *KHrvResultAvg     SDNN平均值
         *KHrvResultTotal   总SDNN结果
         *KHrvResultCount   有效数据个数
         *KHrvResultTired   疲劳指数据
         *KHrvResultValue   心率值
         */
        public const String KHrvResultState = "hrvResultState";
        public const String KHrvResultAvg = "hrvResultAvg";
        public const String KHrvResultTotal = "hrvResultTotal";
        public const String KHrvResultCount = "hrvResultCount";
        public const String KHrvResultTired = "hrvResultTired";
        public const String KHrvResultValue = "hrvResultValue";


        /*
         *KDisturbState     1:开始运动   0：停止运动
         *KSlipHand         1: 带在手上   0;脱手。
         *KPPGData          PPG的波型值
         */
        public const String KDisturbState = "disturbState";
        public const String KSlipHand = "slipHand";
        public const String KPPGData = "ppgData";
        public const String KPPIData = "ppiData";

        public const String ppgResult = "ppgResult";

        public const String ppgStartSucessed="ppgStartSucessed";
        public const String ppgStartFailed="ppgStartFailed";
        public const String ppgStop="ppgStop";
        public const String ppgQuit="ppgQuit";
        public const String ppgMeasurementProgress="ppgMeasurementProgress";





        /*
         *@param Date       时间：年月日时分秒
         *@param Latitude   纬度数据
         *@param Longitude  经度数据
         */


        public const String Latitude = "locationLatitude";
        public const String Longitude = "locationLongitude";

        public const String KActivityLocationTime = "ActivityLocationTIme";
        public const String KActivityLocationLatitude = "ActivityLocationLatitude";
        public const String KActivityLocationLongitude = "ActivityLocationLongitude";
        public const String KActivityLocationCount = "KActivityLocationCount";
        /*
         *      GET_SPORTMODEL_DATA
         *@param  Date        时间：年月日时分秒
         *@param  ActivityMode 运动类型
         0=Run,
         1=Cycling,
         2=Swimming,
         3=Badminton,
         4=Football,
         5=Tennis,
         6=Yoga,
         7=Medication,
         8=Dance
 
         *@param  HeartRate       心率
         *@param  ActiveMinutes   运动时间
         *@param  Step       运动步数
         *@param  Pace       运动速度
         *@param  Calories    卡路里
         *@param  Distance     距离
         */


        public const String ActivityMode = "sportModel";
        public const String Pace = "sportModelSpeed";

        public const String KDataID = "KDataID";
        public const String KPhoneDataLength = "KPhoneDataLength";
        public const String KClockLast = "KClockLast";


        public const String TakePhotoMode = "TakePhotoMode";
        public const String KFunction_tel = "TelMode";
        public const String KFunction_reject_tel = "RejectTelMode";

        public const String FindMobilePhoneMode = "FindMobilePhoneMode";
        public const String KEnable_exercise = "KEnable_exercise";
        public const String ECGQualityValue = "ECGQualityValue";

        public const String ECGResultValue = "ECGResultVALUE";
        public const String ECGHrvValue = "ECGHrvValue";
        public const String ECGAvBlockValue = "ECGAvBlockValue";
        public const String ECGHrValue = "ECGHrValue";
        public const String PPGHrValue = "PPGHrValue";
        public const String ECGStreesValue = "ECGStreesValue";
        public const String ECGhighBpValue = "ECGhighBpValue";
        public const String ECGLowBpValue = "ECGLowBpValue";
        public const String ECGMoodValue = "ECGMoodValue";
        public const String ECGBreathValue = "ECGBreathValue";
        public const String KEcgDataString = "KEcgDataString"; // ecg
        public const String ECGValue = "ECGValue";
        public const String PPGValue = "PPGValue";
        public const String EcgStatus = "EcgStatus";
        public const String EcgSBP = "PPGSBP";
        public const String EcgDBP = "PPGDBP";
        public const String EcgHR = "PPGHR";
        public const String WaveformDownTime = "WaveformDownTime";
        public const String WaveformRiseTime = "WaveformRiseTime";

        public const String EcgGender = "Gender";
        public const String EcgAge = "Age";
        public const String EcgHeight = "Height";
        public const String EcgWeight = "Weight";
    }
}
