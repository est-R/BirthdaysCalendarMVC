using BirthdayCalendarMVC.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace BirthdayCalendarMVC.Services
{
    public class MongoService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<PersonDTO> _persons;

        public MongoService(IOptions<BirthdaysDatabaseSettings> birthdaysDatabaseSettings)
        {
            var client = new MongoClient(birthdaysDatabaseSettings.Value.ConnectionString);
            _database = client.GetDatabase(birthdaysDatabaseSettings.Value.DatabaseName);
            _persons = _database.GetCollection<PersonDTO>(birthdaysDatabaseSettings.Value.PersonsCollectionName);
        }

        public async Task<List<PersonDTO>> GetAsync() =>
            await _persons.Find(_ => true).ToListAsync();

        public async Task<PersonDTO?> GetAsync(string bsonId) =>
            await _persons.Find(x => x.BsonId == bsonId).FirstOrDefaultAsync();

        public async Task CreateAsync(PersonDTO newPerson) =>
            await _persons.InsertOneAsync(newPerson);

        public async Task UpdateAsync(string bsonId, PersonDTO updatedPerson)
        {
            var filter = Builders<PersonDTO>.Filter
                .Eq(p => p.BsonId, bsonId);
            _persons.ReplaceOne(filter, updatedPerson);
        }

        public async Task RemoveAsync(string bsonId) =>
           await _persons.DeleteOneAsync(x => x.BsonId == bsonId);

        public void RemoveMultiple(List<string> bsonIds)
        {
            foreach (var bsonId in bsonIds)
            {
                RemoveAsync(bsonId);
            }
        }

        internal List<PersonDTO> GetPersonsByName(string name)
        {
            List<PersonDTO> list = GetAsync().Result.Where(person => person.Name == name).ToList();
            return list;
        }

        internal List<PersonDTO> GetTodaysPersons()
        {
            DateTime today = DateTime.Today;
            List<PersonDTO> list = GetAsync().Result.Where(person => person.Date.Month == today.Month && person.Date.Day == today.Day).OrderBy(person => person.Date).ToList();
            return list;
        }

        internal List<PersonDTO> GetNearestPersons()
        {
            List<PersonDTO> list = GetAsync().Result.Where(person => person.Date.DayOfYear > DateTime.Today.DayOfYear
                && person.Date.DayOfYear < DateTime.Today.AddDays(15).DayOfYear)
                .OrderBy(person => person.Date.DayOfYear)
                .ToList();
            return list;
        }
    }
}
