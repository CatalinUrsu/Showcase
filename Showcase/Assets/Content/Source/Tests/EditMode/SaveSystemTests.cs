using System;
using System.IO;
using Helpers;
using NUnit.Framework;

namespace Source.Tests
{
public class SaveSystemTests
{
    [Serializable]
    public class SaveV1
    {
        public int Lvl;
    }

    // Same save in next game version: one more field
    [Serializable]
    public class SaveV2
    {
        public int Lvl;
        public int NewField = 3;
    }

    string _path;

    [SetUp]
    public void SetUp() => _path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");

    [TearDown]
    public void TearDown() => SaveSystem.Delete(_path);

    [Test]
    public void LoadOrCreate_ModelGotNewField_KeepsSavedValues()
    {
        SaveSystem.Save(_path, new SaveV1 { Lvl = 7 });

        var loaded = SaveSystem.LoadOrCreate<SaveV2>(_path);

        Assert.AreEqual(7, loaded.Lvl);
        Assert.AreEqual(3, loaded.NewField);
    }

    [Test]
    public void LoadOrCreate_FileEditedByHand_ReturnsNew()
    {
        SaveSystem.Save(_path, new SaveV1 { Lvl = 7 });
        File.WriteAllText(_path, File.ReadAllText(_path).Replace("\"Lvl\": 7", "\"Lvl\": 99"));

        var loaded = SaveSystem.LoadOrCreate<SaveV1>(_path);

        Assert.AreEqual(0, loaded.Lvl);
    }

    [Test]
    public void LoadOrCreate_OlderVersion_CallsMigrate()
    {
        SaveSystem.Save(_path, new SaveV1 { Lvl = 7 }, version: 1);

        var loaded = SaveSystem.LoadOrCreate<SaveV1>(_path, version: 2, migrate: (data, fromVersion) => data["Lvl"] = 8);

        Assert.AreEqual(8, loaded.Lvl);
    }
}
}
