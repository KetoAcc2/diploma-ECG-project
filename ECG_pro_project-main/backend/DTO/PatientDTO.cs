using System.ComponentModel.DataAnnotations;

public class PatientDTO
{

    [Key]
    public int PatientId { get; set; }
    public String Sex { get; set; } = default!;
    public int Age { get; set; }
    public int bpm { get; set; }

    public int question1 { get; set; }
    public int question2 { get; set; }

    public int czestotliwosckomor { get; set; }
    public int odstepRR { get; set; }
    public int odstepPQ { get; set; }
    public int odstepQT { get; set; }
    public int QTc { get; set; }
    public int QRStime { get; set; }

    public bool five1 { get; set; }

    public bool five2I { get; set; }
    public bool five2II { get; set; }
    public bool five2III { get; set; }
    public bool five2aVR { get; set; }
    public bool five2aVL { get; set; }
    public bool five2aVF { get; set; }
    public bool five2V1 { get; set; }
    public bool five2V2 { get; set; }
    public bool five2V3 { get; set; }
    public bool five2V4 { get; set; }
    public bool five2V5 { get; set; }
    public bool five2V6 { get; set; }

    public bool five3I { get; set; }
    public bool five3II { get; set; }
    public bool five3III { get; set; }
    public bool five3aVR { get; set; }
    public bool five3aVL { get; set; }
    public bool five3aVF { get; set; }
    public bool five3V1 { get; set; }
    public bool five3V2 { get; set; }
    public bool five3V3 { get; set; }
    public bool five3V4 { get; set; }
    public bool five3V5 { get; set; }
    public bool five3V6 { get; set; }

    public bool five4I { get; set; }
    public bool five4II { get; set; }
    public bool five4III { get; set; }
    public bool five4aVR { get; set; }
    public bool five4aVL { get; set; }
    public bool five4aVF { get; set; }
    public bool five4V1 { get; set; }
    public bool five4V2 { get; set; }
    public bool five4V3 { get; set; }
    public bool five4V4 { get; set; }
    public bool five4V5 { get; set; }
    public bool five4V6 { get; set; }

    public bool five5I { get; set; }
    public bool five5II { get; set; }
    public bool five5III { get; set; }
    public bool five5aVR { get; set; }
    public bool five5aVL { get; set; }
    public bool five5aVF { get; set; }
    public bool five5V1 { get; set; }
    public bool five5V2 { get; set; }
    public bool five5V3 { get; set; }
    public bool five5V4 { get; set; }
    public bool five5V5 { get; set; }
    public bool five5V6 { get; set; }

    public bool six1 { get; set; }

    public bool six2I { get; set; }
    public bool six2II { get; set; }
    public bool six2III { get; set; }
    public bool six2aVR { get; set; }
    public bool six2aVL { get; set; }
    public bool six2aVF { get; set; }
    public bool six2V1 { get; set; }
    public bool six2V2 { get; set; }
    public bool six2V3 { get; set; }
    public bool six2V4 { get; set; }
    public bool six2V5 { get; set; }
    public bool six2V6 { get; set; }

    public bool six3I { get; set; }
    public bool six3II { get; set; }
    public bool six3III { get; set; }
    public bool six3aVR { get; set; }
    public bool six3aVL { get; set; }
    public bool six3aVF { get; set; }
    public bool six3V1 { get; set; }
    public bool six3V2 { get; set; }
    public bool six3V3 { get; set; }
    public bool six3V4 { get; set; }
    public bool six3V5 { get; set; }
    public bool six3V6 { get; set; }

    public bool seven1 { get; set; }

    public bool seven2I { get; set; }
    public bool seven2II { get; set; }
    public bool seven2III { get; set; }
    public bool seven2aVR { get; set; }
    public bool seven2aVL { get; set; }
    public bool seven2aVF { get; set; }
    public bool seven2V1 { get; set; }
    public bool seven2V2 { get; set; }
    public bool seven2V3 { get; set; }
    public bool seven2V4 { get; set; }
    public bool seven2V5 { get; set; }
    public bool seven2V6 { get; set; }

    public bool seven3I { get; set; }
    public bool seven3II { get; set; }
    public bool seven3III { get; set; }
    public bool seven3aVR { get; set; }
    public bool seven3aVL { get; set; }
    public bool seven3aVF { get; set; }
    public bool seven3V1 { get; set; }
    public bool seven3V2 { get; set; }
    public bool seven3V3 { get; set; }
    public bool seven3V4 { get; set; }
    public bool seven3V5 { get; set; }
    public bool seven3V6 { get; set; }

    public bool seven4I { get; set; }
    public bool seven4II { get; set; }
    public bool seven4III { get; set; }
    public bool seven4aVR { get; set; }
    public bool seven4aVL { get; set; }
    public bool seven4aVF { get; set; }
    public bool seven4V1 { get; set; }
    public bool seven4V2 { get; set; }
    public bool seven4V3 { get; set; }
    public bool seven4V4 { get; set; }
    public bool seven4V5 { get; set; }
    public bool seven4V6 { get; set; }

    public bool seven5I { get; set; }
    public bool seven5II { get; set; }
    public bool seven5III { get; set; }
    public bool seven5aVR { get; set; }
    public bool seven5aVL { get; set; }
    public bool seven5aVF { get; set; }
    public bool seven5V1 { get; set; }
    public bool seven5V2 { get; set; }
    public bool seven5V3 { get; set; }
    public bool seven5V4 { get; set; }
    public bool seven5V5 { get; set; }
    public bool seven5V6 { get; set; }

    public bool seven6I { get; set; }
    public bool seven6II { get; set; }
    public bool seven6III { get; set; }
    public bool seven6aVR { get; set; }
    public bool seven6aVL { get; set; }
    public bool seven6aVF { get; set; }
    public bool seven6V1 { get; set; }
    public bool seven6V2 { get; set; }
    public bool seven6V3 { get; set; }
    public bool seven6V4 { get; set; }
    public bool seven6V5 { get; set; }
    public bool seven6V6 { get; set; }

    public bool eight1 { get; set; }

    public bool eight2I { get; set; }
    public bool eight2II { get; set; }
    public bool eight2III { get; set; }
    public bool eight2aVR { get; set; }
    public bool eight2aVL { get; set; }
    public bool eight2aVF { get; set; }
    public bool eight2V1 { get; set; }
    public bool eight2V2 { get; set; }
    public bool eight2V3 { get; set; }
    public bool eight2V4 { get; set; }
    public bool eight2V5 { get; set; }
    public bool eight2V6 { get; set; }

    public bool eight3I { get; set; }
    public bool eight3II { get; set; }
    public bool eight3III { get; set; }
    public bool eight3aVR { get; set; }
    public bool eight3aVL { get; set; }
    public bool eight3aVF { get; set; }
    public bool eight3V1 { get; set; }
    public bool eight3V2 { get; set; }
    public bool eight3V3 { get; set; }
    public bool eight3V4 { get; set; }
    public bool eight3V5 { get; set; }
    public bool eight3V6 { get; set; }

    public bool eight4I { get; set; }
    public bool eight4II { get; set; }
    public bool eight4III { get; set; }
    public bool eight4aVR { get; set; }
    public bool eight4aVL { get; set; }
    public bool eight4aVF { get; set; }
    public bool eight4V1 { get; set; }
    public bool eight4V2 { get; set; }
    public bool eight4V3 { get; set; }
    public bool eight4V4 { get; set; }
    public bool eight4V5 { get; set; }
    public bool eight4V6 { get; set; }

    public bool eight5I { get; set; }
    public bool eight5II { get; set; }
    public bool eight5III { get; set; }
    public bool eight5aVR { get; set; }
    public bool eight5aVL { get; set; }
    public bool eight5aVF { get; set; }
    public bool eight5V1 { get; set; }
    public bool eight5V2 { get; set; }
    public bool eight5V3 { get; set; }
    public bool eight5V4 { get; set; }
    public bool eight5V5 { get; set; }
    public bool eight5V6 { get; set; }

    public bool eight6I { get; set; }
    public bool eight6II { get; set; }
    public bool eight6III { get; set; }
    public bool eight6aVR { get; set; }
    public bool eight6aVL { get; set; }
    public bool eight6aVF { get; set; }
    public bool eight6V1 { get; set; }
    public bool eight6V2 { get; set; }
    public bool eight6V3 { get; set; }
    public bool eight6V4 { get; set; }
    public bool eight6V5 { get; set; }
    public bool eight6V6 { get; set; }

    public bool eight7I { get; set; }
    public bool eight7II { get; set; }
    public bool eight7III { get; set; }
    public bool eight7aVR { get; set; }
    public bool eight7aVL { get; set; }
    public bool eight7aVF { get; set; }
    public bool eight7V1 { get; set; }
    public bool eight7V2 { get; set; }
    public bool eight7V3 { get; set; }
    public bool eight7V4 { get; set; }
    public bool eight7V5 { get; set; }
    public bool eight7V6 { get; set; }

    public bool eight8I { get; set; }
    public bool eight8II { get; set; }
    public bool eight8III { get; set; }
    public bool eight8aVR { get; set; }
    public bool eight8aVL { get; set; }
    public bool eight8aVF { get; set; }
    public bool eight8V1 { get; set; }
    public bool eight8V2 { get; set; }
    public bool eight8V3 { get; set; }
    public bool eight8V4 { get; set; }
    public bool eight8V5 { get; set; }
    public bool eight8V6 { get; set; }

    public bool eight9I { get; set; }
    public bool eight9II { get; set; }
    public bool eight9III { get; set; }
    public bool eight9aVR { get; set; }
    public bool eight9aVL { get; set; }
    public bool eight9aVF { get; set; }
    public bool eight9V1 { get; set; }
    public bool eight9V2 { get; set; }
    public bool eight9V3 { get; set; }
    public bool eight9V4 { get; set; }
    public bool eight9V5 { get; set; }
    public bool eight9V6 { get; set; }

    public bool eight10I { get; set; }
    public bool eight10II { get; set; }
    public bool eight10III { get; set; }
    public bool eight10aVR { get; set; }
    public bool eight10aVL { get; set; }
    public bool eight10aVF { get; set; }
    public bool eight10V1 { get; set; }
    public bool eight10V2 { get; set; }
    public bool eight10V3 { get; set; }
    public bool eight10V4 { get; set; }
    public bool eight10V5 { get; set; }
    public bool eight10V6 { get; set; }

    public bool nine1 { get; set; }

    public bool nine2I { get; set; }
    public bool nine2II { get; set; }
    public bool nine2III { get; set; }
    public bool nine2aVR { get; set; }
    public bool nine2aVL { get; set; }
    public bool nine2aVF { get; set; }
    public bool nine2V1 { get; set; }
    public bool nine2V2 { get; set; }
    public bool nine2V3 { get; set; }
    public bool nine2V4 { get; set; }
    public bool nine2V5 { get; set; }
    public bool nine2V6 { get; set; }

    public bool nine3I { get; set; }
    public bool nine3II { get; set; }
    public bool nine3III { get; set; }
    public bool nine3aVR { get; set; }
    public bool nine3aVL { get; set; }
    public bool nine3aVF { get; set; }
    public bool nine3V1 { get; set; }
    public bool nine3V2 { get; set; }
    public bool nine3V3 { get; set; }
    public bool nine3V4 { get; set; }
    public bool nine3V5 { get; set; }
    public bool nine3V6 { get; set; }

    public bool nine4I { get; set; }
    public bool nine4II { get; set; }
    public bool nine4III { get; set; }
    public bool nine4aVR { get; set; }
    public bool nine4aVL { get; set; }
    public bool nine4aVF { get; set; }
    public bool nine4V1 { get; set; }
    public bool nine4V2 { get; set; }
    public bool nine4V3 { get; set; }
    public bool nine4V4 { get; set; }
    public bool nine4V5 { get; set; }
    public bool nine4V6 { get; set; }

    public bool nine5I { get; set; }
    public bool nine5II { get; set; }
    public bool nine5III { get; set; }
    public bool nine5aVR { get; set; }
    public bool nine5aVL { get; set; }
    public bool nine5aVF { get; set; }
    public bool nine5V1 { get; set; }
    public bool nine5V2 { get; set; }
    public bool nine5V3 { get; set; }
    public bool nine5V4 { get; set; }
    public bool nine5V5 { get; set; }
    public bool nine5V6 { get; set; }

    public bool nine6I { get; set; }
    public bool nine6II { get; set; }
    public bool nine6III { get; set; }
    public bool nine6aVR { get; set; }
    public bool nine6aVL { get; set; }
    public bool nine6aVF { get; set; }
    public bool nine6V1 { get; set; }
    public bool nine6V2 { get; set; }
    public bool nine6V3 { get; set; }
    public bool nine6V4 { get; set; }
    public bool nine6V5 { get; set; }
    public bool nine6V6 { get; set; }

    public bool ten1 { get; set; }

    public bool ten2I { get; set; }
    public bool ten2II { get; set; }
    public bool ten2III { get; set; }
    public bool ten2aVR { get; set; }
    public bool ten2aVL { get; set; }
    public bool ten2aVF { get; set; }
    public bool ten2V1 { get; set; }
    public bool ten2V2 { get; set; }
    public bool ten2V3 { get; set; }
    public bool ten2V4 { get; set; }
    public bool ten2V5 { get; set; }
    public bool ten2V6 { get; set; }

    public bool eleven1 { get; set; }
    public bool eleven2 { get; set; }
    public bool eleven3 { get; set; }
    public bool eleven4 { get; set; }
    public bool eleven5 { get; set; }
    public bool eleven6 { get; set; }
    public bool eleven7 { get; set; }

    public bool twelve1pojedyncza { get; set; }
    public bool twelve1mnoga { get; set; }
    public bool twelve1pary { get; set; }
    public bool twelve2pojedyncza { get; set; }
    public bool twelve2mnoga { get; set; }
    public bool twelve2pary { get; set; }

    public bool thirteen1 { get; set; }
    public bool thirteenAA1 { get; set; }
    public bool thirteenAA2 { get; set; }
    public bool thirteenAA3 { get; set; }
    public bool thirteenAA4 { get; set; }
    public bool thirteenAA5 { get; set; }
    public bool thirteenAA6 { get; set; }
    public bool thirteenAA7 { get; set; }

    public bool fourteen1 { get; set; }
    public bool fourteen2leftfront { get; set; }
    public bool fourteen2leftside { get; set; }
    public bool fourteen2leftdown { get; set; }
    public bool fourteen2right { get; set; }
    public bool fourteen3leftfront { get; set; }
    public bool fourteen3leftside { get; set; }
    public bool fourteen3leftdown { get; set; }
    public bool fourteen3right { get; set; }
    public bool fourteen4leftfront { get; set; }
    public bool fourteen4leftside { get; set; }
    public bool fourteen4leftdown { get; set; }
    public bool fourteen4right { get; set; }
    public bool fourteen5 { get; set; }

    public bool fifteen1 { get; set; }
    public bool fifteen2 { get; set; }
    public bool fifteen3 { get; set; }
    public bool fifteen4 { get; set; }
    public bool fifteen5 { get; set; }
    public bool fifteen6 { get; set; }
    public bool fifteen7 { get; set; }
    public bool fifteen8 { get; set; }
    public bool fifteen9 { get; set; }
    public bool fifteen10 { get; set; }
    public bool fifteen11 { get; set; }
    public bool fifteen12 { get; set; }
    public bool fifteen13 { get; set; }
    public bool fifteen14 { get; set; }
    public bool fifteen15 { get; set; }
    public bool fifteen16 { get; set; }
    public bool fifteen17 { get; set; }
    public bool fifteen18 { get; set; }
}
