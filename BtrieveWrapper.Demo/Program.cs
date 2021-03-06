﻿using System;
using System.Collections.Generic;
using BtrieveWrapper.Orm;
using BtrieveWrapper.Orm.Models.CustomModels;

namespace BtrieveWrapper.Demo
{
    class Program
    {
        static void Main(string[] args) {
            DemodataDbClient client = new DemodataDbClient();

            Console.WriteLine("[Read people whose last initial is 'D']");
            using (RecordManager<Person, PersonKeyCollection> people = client.Person()) {
                IEnumerable<Person> query = people.Query(p =>
                    p.Last_Name.GreaterThanOrEqual("D") &&
                    p.Last_Name.LessThan("E"));
                foreach (Person person in query) {
                    Console.WriteLine("Name: {0} {1}",
                        person.First_Name,
                        person.Last_Name);
                }
            }

            Console.WriteLine();

            Console.WriteLine("[Person CRUD]");
            using (RecordManager<Person, PersonKeyCollection> people = client.Person()) {
                using (Transaction transaction = client.BeginTransaction()) {
                    Console.Write("Create person: ");
                    Person person = new Person();
                    person.ID = 0;
                    person.First_Name = "Ieyasu";
                    person.Last_Name = "Tokugawa";
                    people.Add(person);
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.GetAndManage(p => p.ID == 0);
                    Console.WriteLine("Read person: " + 
                        (person == null ? "not found" : person.First_Name + " " + person.Last_Name));

                    Console.Write("Update person: ");
                    person.First_Name = "Iemitsu";
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.GetAndManage(p => p.ID == 0);
                    Console.WriteLine("Read person: " + 
                        (person == null ? "not found" : person.First_Name + " " + person.Last_Name));

                    Console.Write("Delete person: ");
                    people.Remove(person);
                    people.SaveChanges();
                    Console.WriteLine("done");
                    people.Detach(person);

                    person = people.Get(p => p.ID == 0);
                    Console.WriteLine("Read person: " + 
                        (person == null ? "not found" : person.First_Name + " " + person.Last_Name));

                    transaction.Commit();
                }
            }
        }
    }
}
