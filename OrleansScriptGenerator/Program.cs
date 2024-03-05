using System;
using Orleans.Persistence.AdoNet;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(AdoNetGrainStorageOptions.GetCreateTableScriptForRelationalVendor(AdoNetInvariants.InvariantNamePostgreSql));
    }
}