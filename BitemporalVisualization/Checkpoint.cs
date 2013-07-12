using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitemporalVisualization
{
    public class Checkpoint
    {
        private Dictionary<long, Version> transactions;

        public Tuple<Version, Version> MinCorner()
        {
            DateTime minValidDateTime = DateTime.MaxValue;
            DateTime minRecordDateTime = DateTime.MaxValue;
            Version minRecord = transactions.Values.First();
            Version minValid = transactions.Values.First();
            foreach (var ver in transactions)
            {
                if (ver.Value.recordFrom < minRecordDateTime && ver.Value.recordFrom != DateTime.MinValue)
                {
                    minRecord = ver.Value;
                    minRecordDateTime = ver.Value.recordFrom;
                }
                if (ver.Value.validFrom < minValidDateTime && ver.Value.validFrom != DateTime.MinValue)
                {
                    minValid = ver.Value;
                    minValidDateTime = ver.Value.validFrom;
                }
            }
            return Tuple.Create(minRecord, minValid);
        }

        public Checkpoint()
        {
            transactions = new Dictionary<long, Version>();

            // If you're working from home, comment this stuff out and just do a series of versions.Add
            // for test data
            var sqlConn = new SqlConnection(ConfigurationManager.AppSettings["dbConnString"]);
            sqlConn.Open();

            var commandString =
                "SELECT * FROM Transactions JOIN Revision_Transaction_TC ON Id = TransactionId WHERE Id IN (SELECT Id FROM Transactions T JOIN Revision_Transaction_TC RT ON RT.TransactionId = T.Id JOIN Entity_Status_T ES ON RT.RevisionId = ES.RevisionId WHERE EntityId = {0})";

            var command = sqlConn.CreateCommand();
            command.CommandText = String.Format(commandString, 33);
            command.Prepare();
            var dataReader = command.ExecuteReader();

            Dictionary<long, Version> versions = new Dictionary<long, Version>();

            while(dataReader.Read())
            {
                var tranId = dataReader.GetInt64(0);
                var creator = dataReader.GetInt32(1);
                var recordFrom = dataReader.GetDateTime(2);
                var recordTo = dataReader.GetDateTime(3);
                var validFrom = dataReader.GetDateTime(4);
                if (validFrom.Year == 1753)
                    validFrom = DateTime.MinValue;
                var validTo = dataReader.GetDateTime(5);
                var revId = dataReader.GetInt64(6);
                if (!versions.ContainsKey(tranId))
                {
                    versions.Add(tranId, new Version(tranId, revId, recordFrom, recordTo, validFrom, validTo));
                }
                else
                {
                    if (versions[tranId].recordFrom != recordFrom)
                        throw new Exception();
                    if (versions[tranId].recordTo != recordTo)
                        throw new Exception();
                    if (versions[tranId].validFrom != validFrom)
                        throw new Exception();
                    if (versions[tranId].validTo != validTo)
                        throw new Exception();
                    versions[tranId].revIds.Add(revId);
                }
            }

            foreach (var ver in versions)
                transactions.Add(ver.Key, ver.Value);
        }

        public Version FindVersion(CoordinateTransformer coords, int x, int y)
        {
            var validTime = coords.XToValidTime(x);
            var recordTime = coords.YToRecordTime(y);
            return transactions.SingleOrDefault(kvp => kvp.Value.recordFrom <= recordTime &&
                                                kvp.Value.recordTo >= recordTime &&
                                                kvp.Value.validFrom <= validTime &&
                                                kvp.Value.validTo >= validTime).Value;
        }

        public void AddVersion(Version v)
        {
            transactions.Add(v.transactionId, v);
        }

        public void Draw(CoordinateTransformer coordinateSystem, BrushProvider brushProvider, Graphics graphicsObj)
        {
            foreach (var version in transactions.Values)
            {
                version.Draw(coordinateSystem, brushProvider, graphicsObj);
            }
        }
    }
}
