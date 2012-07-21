using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;

namespace WP7.TapDance.Model
{
    public class ScoreStorage
    {
        private const string scoresFilePath = "Scores.xml";
        private readonly XmlWriterSettings xmlWriterSettings;

        private List<Score> tempList; 

        public ScoreStorage()
        {
            xmlWriterSettings = new XmlWriterSettings { Indent = true };
            tempList = new List<Score>(){ new Score(0.300, DateTime.Now),
            new Score(0.150, DateTime.Now)};
        }


        public void AddScore(Score score)
        {
            List<Score> scores = GetScores().ToList();
            scores.Add(score);
            SaveScores(scores);
            tempList.Add(score);
        }

        private void SaveScores(IEnumerable<Score> scores)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (
                    IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(scoresFilePath, FileMode.OpenOrCreate)
                    )
                {
                    var serializer = new XmlSerializer(typeof(List<Score>));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        serializer.Serialize(xmlWriter, scores);
                    }
                }
            }
        }

        public IEnumerable<Score> GetScores()
        {
            return tempList;
            var scores = new List<Score>();

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(scoresFilePath))
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile(scoresFilePath, FileMode.Open))
                    {
                        try
                        {
                            var serializer = new XmlSerializer(typeof(List<Score>));
                            scores = (List<Score>)serializer.Deserialize(stream);
                        }
                        catch /*omnomnom*/
                        {
                        }
                    }
                }
            }

            return scores;
        }
    }
}
