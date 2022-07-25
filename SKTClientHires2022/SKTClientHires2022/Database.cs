using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using static Android.InputMethodServices.Keyboard;

namespace SKTClientHires2022
{
    public class Database
    {
        private readonly SQLiteAsyncConnection database;

        public Database(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTablesAsync<Person, Member>();
        }

        public CreateTableResult DropAndCreateNewPersonTableAsync()
        {
            database.DropTableAsync<Person>();
            database.CreateTableAsync<Person>();
            return SQLite.CreateTableResult.Created;
        }
        public Task<int> SavePersonAsync(Person person)
        {
            return database.InsertAsync(person);
        }
        public Task<int> SaveMemberAsync(Member member)
        {
            return database.InsertAsync(member);
        }

        public Task<int> UpdatePersonAsync(Person person)
        {
            return database.UpdateAsync(person);
        }
        public Task<int> UpdateMemberAsync(Member member)
        {
            return database.UpdateAsync(member);
        }
        public Task<int> DeletePersonAsync(Person person)
        {
            return database.DeleteAsync(person);
        }
        public Task<int> DeleteMemberAsync(Member member)
        {
            return database.DeleteAsync(member);
        }
        //queries
        public Task<List<Person>> GetPersonsAsync()
        {
            return database.Table<Person>().ToListAsync();
        }
        public Task<List<Member>> GetMembersAsync()
        {
            return database.Table<Member>().ToListAsync();
        }
        public Task<List<Person>> LinqAllRecordsAsync()
        {
            return database.Table<Person>().Where(row => row.Id >= 0).ToListAsync();
        }
        public async Task<List<Person>> LinqSeeCurrentOrCompletedHires(bool queryCurrent, TimeSpan ts)
        {
            //TimeSpan _ts = ts;
            if (queryCurrent)
            {
                return await database.Table<Person>().Where(row => row.TimeOutHr >= ts.Hours).ToListAsync();

            }
            else
            {
                //Person hireTimeHour = await database.Table<Person>().Where(row => (row.TimeOutHr < ts.Hours));
                if (ts.Minutes == 0)
                {
                    
                }
                List<Person> listPersonMoreThanOneHour = await database.Table<Person>().Where(row => row.TimeOutHr < ts.Hours).ToListAsync();
                List<Person> ListPersonWithOneHourAndMins = await database.Table<Person>().Where(row => row.TimeOutHr <= ts.Hours && row.TimeOutMin < ts.Minutes).ToListAsync();

                return listPersonMoreThanOneHour.Concat(ListPersonWithOneHourAndMins).ToList();
                
                //return await database.Table<Person>().Where(row => row.TimeOutHr <= ts.Hours && row.TimeOutMin < ts.Minutes).ToListAsync();
            }
        }

        public Task<List<Person>> LinqAgeNotOKHiresAsync(bool child)
        {
            TimeSpan ts = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            if(child)
            {
//wow - i heart linq instead of queries row.TimeOutHr <= ts.Hours && row.TimeOutMin < ts.Minutes)
                return database.Table<Person>().Where(p => p.AgeNotOK == true && p.TimeOutHr >= ts.Hours && p.TimeOutMin >= ts.Minutes).ToListAsync();
}
else
{
                return database.Table<Person>().Where(p => p.AgeNotOK == false && p.TimeOutHr >= ts.Hours && p.TimeOutMin >= ts.Minutes).ToListAsync();
            }
        }
        public Task<List<Person>> LinqSingleHireAsync(Person person)
        {
            return database.Table<Person>().Where(p => p.Id == person.Id).ToListAsync();
        }
        public Task<List<Member>> LinqMemberPasscodesAsync()
        {
            return database.Table<Member>().Where(p => p.Pass != null).ToListAsync();
        }
    }
}
