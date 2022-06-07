using System.Collections.Generic;

namespace HospitalPatientRecords.MVVM.Model;

public class VarsDictionary
{
    public enum Key
    {
        CURRENT_DOCTOR
    }

    public static Dictionary<Key, object> varsDictionary = new Dictionary<Key, object>();
}