using System;
using System.Collections.Generic;
using Bogus;
using ExtendableEnums.Testing.Models;

namespace ExtendableEnums.EntityFrameworkCore.UnitTests.Entities;

public static class SamplePersonEntityFactory
{
    public static IList<SamplePersonEntity> Generate(int count)
    {
        return new Faker<SamplePersonEntity>()
            .RuleFor(e => e.Name, (f, e) => f.Person.FullName)
            .RuleFor(e => e.Id, (f, e) => f.Random.Guid().ToString())
            .RuleFor(e => e.Status, (f, e) => f.PickRandomParam(SampleStatus.GetAll()))
            .Generate(count);
    }

    public static void Initialize()
    {
        Randomizer.Seed = new Random(62392);
    }
}