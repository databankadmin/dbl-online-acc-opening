using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PdfFillerDemo.Models
{

    public class ResponseModel
    {
        public string filetype { get; set; }
        public string sha1 { get; set; }
        public string ssdeep { get; set; }
        public int size { get; set; }
        public Exif exif { get; set; }
        public string sha512 { get; set; }
        public string extension { get; set; }
        public Virustotal virustotal { get; set; }
        public string md5 { get; set; }
        public DateTime added_timestamp { get; set; }
        public Trid trid { get; set; }
        public string mimetype { get; set; }
        public string sha256 { get; set; }
        public string sha384 { get; set; }
        public string sha224 { get; set; }
        public int response { get; set; }
    }

    public class Exif
    {
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string FileTypeExtension { get; set; }
        public string MIMEType { get; set; }
        public string ZipBitFlag { get; set; }
        public string ZipCRC { get; set; }
        public int ZipCompressedSize { get; set; }
        public string ZipCompression { get; set; }
        public string ZipFileName { get; set; }
        public string ZipModifyDate { get; set; }
        public int ZipRequiredVersion { get; set; }
        public int ZipUncompressedSize { get; set; }
    }

    public class Virustotal
    {
        public Scans scans { get; set; }
        public string scan_date { get; set; }
        public string permalink { get; set; }
        public int total { get; set; }
        public int positives { get; set; }
    }

    public class Scans
    {
        public Mcafee McAfee { get; set; }
        public Totaldefense TotalDefense { get; set; }
        public Rising Rising { get; set; }
        public TACHYON TACHYON { get; set; }
        public Alyac ALYac { get; set; }
        public AntiyAVL AntiyAVL { get; set; }
        public Malwarebytes Malwarebytes { get; set; }
        public Drweb DrWeb { get; set; }
        public AdAware AdAware { get; set; }
        public Baidu Baidu { get; set; }
        public Cyren Cyren { get; set; }
        public Fortinet Fortinet { get; set; }
        public Qihoo360 Qihoo360 { get; set; }
        public Avira Avira { get; set; }
        public Zonealarm ZoneAlarm { get; set; }
        public Zoner Zoner { get; set; }
        public Kingsoft Kingsoft { get; set; }
        public Gdata GData { get; set; }
        public Yandex Yandex { get; set; }
        public Alibaba Alibaba { get; set; }
        public FProt FProt { get; set; }
        public Sentinelone SentinelOne { get; set; }
        public CMC CMC { get; set; }
        public Sophos Sophos { get; set; }
        public Emsisoft Emsisoft { get; set; }
        public Symantec Symantec { get; set; }
        public CATQuickheal CATQuickHeal { get; set; }
        public VIPRE VIPRE { get; set; }
        public FSecure FSecure { get; set; }
        public Clamav ClamAV { get; set; }
        public MAX MAX { get; set; }
        public NANOAntivirus NANOAntivirus { get; set; }
        public Virobot ViRobot { get; set; }
        public Trustlook Trustlook { get; set; }
        public Ikarus Ikarus { get; set; }
        public K7antivirus K7AntiVirus { get; set; }
        public Bitdefender BitDefender { get; set; }
        public Maxsecure MaxSecure { get; set; }
        public McafeeGWEdition McAfeeGWEdition { get; set; }
        public ESETNOD32 ESETNOD32 { get; set; }
        public VBA32 VBA32 { get; set; }
        public Tencent Tencent { get; set; }
        public Trendmicro TrendMicro { get; set; }
        public Sangfor Sangfor { get; set; }
        public Microsoft Microsoft { get; set; }
        public Bitdefendertheta BitDefenderTheta { get; set; }
        public Bkav Bkav { get; set; }
        public Symantecmobileinsight SymantecMobileInsight { get; set; }
        public Superantispyware SUPERAntiSpyware { get; set; }
        public Panda Panda { get; set; }
        public AvastMobile AvastMobile { get; set; }
        public AVG AVG { get; set; }
        public Zillya Zillya { get; set; }
        public Avast Avast { get; set; }
        public Comodo Comodo { get; set; }
        public Kaspersky Kaspersky { get; set; }
        public AhnlabV3 AhnLabV3 { get; set; }
        public Aegislab AegisLab { get; set; }
        public Arcabit Arcabit { get; set; }
        public TrendmicroHousecall TrendMicroHouseCall { get; set; }
        public K7GW K7GW { get; set; }
        public MicroworldEscan MicroWorldeScan { get; set; }
        public Fireeye FireEye { get; set; }
        public Jiangmin Jiangmin { get; set; }
    }

    public class Mcafee
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Totaldefense
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Rising
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class TACHYON
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Alyac
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class AntiyAVL
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Malwarebytes
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Drweb
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class AdAware
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Baidu
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Cyren
    {
        public int update { get; set; }
        public string result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Fortinet
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Qihoo360
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Avira
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Zonealarm
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class Zoner
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Kingsoft
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Gdata
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Yandex
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Alibaba
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class FProt
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Sentinelone
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class CMC
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Sophos
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Emsisoft
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Symantec
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class CATQuickheal
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class VIPRE
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class FSecure
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Clamav
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class MAX
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class NANOAntivirus
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Virobot
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Trustlook
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class Ikarus
    {
        public int update { get; set; }
        public string result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class K7antivirus
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Bitdefender
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public float version { get; set; }
    }

    public class Maxsecure
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class McafeeGWEdition
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class ESETNOD32
    {
        public int update { get; set; }
        public string result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class VBA32
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Tencent
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Trendmicro
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Sangfor
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class Microsoft
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Bitdefendertheta
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Bkav
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Symantecmobileinsight
    {
        public int update { get; set; }
        public string result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class Superantispyware
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Panda
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class AvastMobile
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class AVG
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Zillya
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Avast
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Comodo
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public int version { get; set; }
    }

    public class Kaspersky
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class AhnlabV3
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Aegislab
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public float version { get; set; }
    }

    public class Arcabit
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class TrendmicroHousecall
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class K7GW
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class MicroworldEscan
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Fireeye
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Jiangmin
    {
        public int update { get; set; }
        public object result { get; set; }
        public bool detected { get; set; }
        public string version { get; set; }
    }

    public class Trid
    {
        public string _0 { get; set; }
        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
    }

}