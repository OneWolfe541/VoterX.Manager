//using System;
//using System.Windows;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VoterX.Core.Repositories;
//using VoterX.Core.Models.ViewModels;
//using VoterX.Core.Models.Database;

//namespace VoterX.Methods
//{
//    public static class BatchDataMethods
//    {
//        /// <summary>
//        /// Repository for avBatchedVoters
//        /// </summary>
//        public static BatchedVoterRepo BatchedVoters
//        {
//            get
//            {
//                return ((App)Application.Current).voterContainer.Resolve<BatchedVoterRepo>();
//            }
//        }

//        /// <summary>
//        /// Repository for avBatches
//        /// </summary>
//        public static BatchRepo Batches
//        {
//            get
//            {
//                return ((App)Application.Current).voterContainer.Resolve<BatchRepo>();
//            }
//        }        

//        public static bool InsertBatch(avBatch batchToAdd)
//        {
//            bool result = false;
//            using (VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>())
//            {
//                avBatch newBatch = new avBatch();
//                newBatch = batchToAdd;
//                //try
//                //{
//                    _VoterDB.avBatches.Add(newBatch);
//                    _VoterDB.SaveChanges();
//                    result = true;
//                //}
//                //catch
//                //{
//                //    _VoterDB.Dispose();
//                //}
//            }
//            return result;
//        }

//        public static bool CloseBatch(int batchId)
//        {
//            bool result = false;
//            using (VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>())
//            {
//                var batchToClose = _VoterDB.avBatches.Find(batchId);
//                if (batchToClose != null)
//                {
//                    batchToClose.batch_open = false;
//                    _VoterDB.SaveChanges();
//                    result = true;
//                }
//            }

//            return result;
//        }

//        public static bool InsertBatchedVoter(int batchId, int voterId)
//        {
//            bool result = false;
//            using (VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>())
//            {
//                avBatchedVoter newBatchedVoter = new avBatchedVoter
//                {
//                    batch_id = batchId,
//                    voter_id = voterId,
//                    date_added = DateTime.Now
//                };
                
//                //try
//                //{
//                _VoterDB.avBatchedVoters.Add(newBatchedVoter);
//                _VoterDB.SaveChanges();
//                result = true;
//                //}
//                //catch
//                //{
//                //    _VoterDB.Dispose();
//                //}
//            }
//            return result;
//        }

//        public static bool RemoveBatchedVoter(int voterId)
//        {
//            bool result = false;
//            using (VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>())
//            {
//                var voterToRemove = _VoterDB.avBatchedVoters.Find(voterId);
//                if (voterToRemove != null)
//                {
//                    _VoterDB.avBatchedVoters.Remove(voterToRemove);
//                    _VoterDB.SaveChanges();
//                    result = true;
//                }
//            }

//            return result;
//        }

//        public static async void RemoveAllVotersFromBatch(int batchId)
//        {
            
//            using (VoterDatabase _VoterDB = ((App)Application.Current).voterContainer.Resolve<VoterDatabase>())
//            {
//                await _VoterDB.Database.ExecuteSqlCommandAsync(@"DELETE FROM avBatchedVoters WHERE batch_id = " + batchId);
//            }
//        }

//        public static async Task<avBatch> GetBatchAsync(int batchType)
//        {
//            return await Task.Run(() => BatchDataMethods.Batches.Query(0).Where(b =>
//                b.election_id == AppSettings.Election.ElectionID &&
//                b.county_code == AppSettings.Election.CountyCode &&
//                b.poll_id == AppSettings.System.SiteID &&
//                b.computer == AppSettings.System.MachineID &&
//                b.log_code == batchType &&
//                b.batch_open == true).FirstOrDefault());
//        }

//        public static void CreateNewBatch(int batchType)
//        {
//            var newBatch = new avBatch
//            {
//                election_id = AppSettings.Election.ElectionID,
//                county_code = AppSettings.Election.CountyCode,
//                poll_id = AppSettings.System.SiteID,
//                computer = AppSettings.System.MachineID,
//                log_code = batchType,
//                batch_open = true,
//                batch_created_date = DateTime.Now
//            };

//            BatchDataMethods.InsertBatch(newBatch);
//        }

//        public static async void CreateNewBatchAsync(int batchType)
//        {
//                var newBatch = new avBatch
//                {
//                    election_id = AppSettings.Election.ElectionID,
//                    county_code = AppSettings.Election.CountyCode,
//                    poll_id = AppSettings.System.SiteID,
//                    computer = AppSettings.System.MachineID,
//                    log_code = batchType,
//                    batch_open = true,
//                    batch_created_date = DateTime.Now
//                };

//                await Task.Run(() => BatchDataMethods.InsertBatch(newBatch));            
//        }
//    }
//}
