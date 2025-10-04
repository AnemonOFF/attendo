using System.Collections.Concurrent;
using Atendo.Domain.Entities;

public static class AppStorage
{
    private static int _groupSeq = 0;
    private static int _studentSeq = 0;

    private static readonly ConcurrentDictionary<int, Group> _groups = new();

    public static Group CreateGroup(string title)
    {
        var g = new Group { Id = Interlocked.Increment(ref _groupSeq), Title = title, Students = new() };
        _groups[g.Id] = g;
        return Clone(g);
    }

    public static Group? GetGroup(int id) => _groups.TryGetValue(id, out var g) ? Clone(g) : null;

    public static Group? UpdateGroup(Group incoming)
    {
        if (!_groups.TryGetValue(incoming.Id, out var g)) return null;

        g.Title = incoming.Title.Trim();

        foreach (var s in incoming.Students)
        {
            if (s.Id == 0) s.Id = Interlocked.Increment(ref _studentSeq);
            s.GroupId = g.Id;
        }
        g.Students = incoming.Students.ToList();
        return Clone(g);
    }

    public static Student? AddStudent(int groupId, string fullName)
    {
        if (!_groups.TryGetValue(groupId, out var g)) return null;
        var s = new Student
        {
            Id = Interlocked.Increment(ref _studentSeq),
            FullName = fullName.Trim(),
            GroupId = groupId
        };
        g.Students.Add(s);
        return Clone(s);
    }

    public static Student? UpdateStudent(int groupId, int studentId, string fullName)
    {
        if (!_groups.TryGetValue(groupId, out var g)) return null;
        var s = g.Students.FirstOrDefault(x => x.Id == studentId);
        if (s is null) return null;
        s.FullName = fullName.Trim();
        return Clone(s);
    }

    public static bool RemoveStudent(int groupId, int studentId)
    {
        if (!_groups.TryGetValue(groupId, out var g)) return false;
        var removed = g.Students.RemoveAll(x => x.Id == studentId) > 0;
        return removed;
    }

    private static Group Clone(Group g) => new()
    {
        Id = g.Id,
        Title = g.Title,
        Students = g.Students.Select(Clone).ToList()
    };
    private static Student Clone(Student s) => new()
    {
        Id = s.Id,
        FullName = s.FullName,
        GroupId = s.GroupId
    };
}
