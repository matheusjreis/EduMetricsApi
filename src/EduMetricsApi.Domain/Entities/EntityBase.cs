namespace EduMetricsApi.Domain.Entities;

public class EntityBase
{
    public int Id { get; set; }

    public void SetBaseEntityValues(EntityBase obj)
    {
        this.Id = obj.Id;
    }
}