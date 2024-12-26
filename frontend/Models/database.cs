using SQLite;

namespace frontend.Models;
public class IdeaDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public IdeaDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<BrainstormInput>().Wait();
        _database.CreateTableAsync<IdeaDetail>().Wait();
    }

    // CRUD operations for BrainstormInput
    public Task<List<BrainstormInput>> GetBrainstormInputsAsync()
    {
        return _database.Table<BrainstormInput>().ToListAsync();
    }

    public Task<int> SaveBrainstormInputAsync(BrainstormInput input)
    {
        if (_database.Table<BrainstormInput>().Where(i => i.Id == input.Id).CountAsync().Result > 0)
        {
            return _database.UpdateAsync(input);
        }
        else
        {
            return _database.InsertAsync(input);
        }
    }

    public Task<int> DeleteBrainstormInputAsync(BrainstormInput input)
    {
        return _database.DeleteAsync(input);
    }

    // CRUD operations for IdeaDetail
    public Task<List<IdeaDetail>> GetIdeaDetailsAsync()
    {
        return _database.Table<IdeaDetail>().ToListAsync();
    }

    public Task<int> SaveIdeaDetailAsync(IdeaDetail idea)
    {
       return _database.InsertAsync(idea);
    }

    public Task<int> DeleteIdeaDetailAsync(IdeaDetail idea)
    {
        return _database.DeleteAsync(idea);
    }
}
