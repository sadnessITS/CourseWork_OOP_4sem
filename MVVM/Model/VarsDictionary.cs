using System.Collections.Generic;

namespace HospitalPatientRecords.MVVM.Model;

public class VarsDictionary
{
    public enum key
    {
        IdActiveUser,
        DoctorFio
    }

    public static Dictionary<key, string> varsDictionary = new Dictionary<key, string>();
}