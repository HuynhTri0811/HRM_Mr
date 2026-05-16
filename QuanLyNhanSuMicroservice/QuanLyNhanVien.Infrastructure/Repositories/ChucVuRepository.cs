using Microsoft.EntityFrameworkCore;
using QuanLyNhanSuMicroservice.Core.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.Command.ChucVu;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Application.UseCases;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Entities;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Domain.Repositories;
using QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Persistence.Data;

namespace QuanLyNhanSuMicroservice.QuanLyNhanVien.Infrastructure.Repositories
{
    public class ChucVuRepository(NhanSuDbContext context) : IChucVuRepository
    {
        public async Task AddAsync(ChucVu entity)
        {
            await context.ChucVus.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid Oid)
        {
            var entity = await context.ChucVus.FindAsync(Oid);
            if (entity != null)
            {
                context.ChucVus.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChucVu?>> GetAllAsync()
        {
            return await context.ChucVus.ToArrayAsync();
        }

        public async Task<ChucVu?> GetByIdAsync(Guid Oid)
        {
            return await context.ChucVus.FindAsync(Oid);
        }

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChucVu entity)
        {
            context.ChucVus.Update(entity);
            await context.SaveChangesAsync();
        }


    }
}
