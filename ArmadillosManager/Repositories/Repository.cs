using ArmadillosManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmadillosManager.Repositories
{
    public class Repository<T> where T : class
    {
        private readonly Sistem21ClubdeportivoContext context;

        public Repository(Sistem21ClubdeportivoContext cx)
        {
            this.context=cx;
        }
        public DbSet<T> GetAll()
        {
            return context.Set<T>();
        }
        public T? GetById(int id)
        {
            return context.Find<T>(id);
        }
        public void Insert(T entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }
        public void Update(T entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }
        public void Delete(T entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }
    }
}
