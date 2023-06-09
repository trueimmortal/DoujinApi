using DoujinApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DoujinApi.Models;

namespace DoujinApi.Services;

/// <summary>
///  Service for the users.
/// </summary>
public class UserService
{
	private readonly IMongoCollection<User> _users;

	/// <summary>
	///  Constructor for the user service.
	/// </summary>
	/// <param name="settings"> Instance of database settings </param>
	public UserService(IOptions<DatabaseSettings> settings)
	{
		var client = new MongoClient(settings.Value.ConnectionString);
		var database = client.GetDatabase(settings.Value.DatabaseName);
		_users = database.GetCollection<User>(settings.Value.UsersCollectionName);
	}

	/// <summary>
	/// Get all the users inside the database.
	/// </summary>
	/// <param name="ct">Cancellation token</param>
	/// <returns>All the users in the database</returns>
	public async Task<List<User>> GetAsync(CancellationToken ct)
	{
		return await _users.Find(user => true).ToListAsync(cancellationToken: ct);
	}

	/// <summary>
	/// Get the count of all the users inside the database.
	/// </summary>
	/// <param name="ct">Cancellation token</param>
	/// <returns>The count of all users in the database.</returns>
	public async Task<int> GetCountAsync(CancellationToken ct)
	{
		return (int) await _users.CountDocumentsAsync(user => true, cancellationToken: ct);
	}

	/// <summary>
	/// Get a user by its user id.
	/// </summary>
	/// <param name="userId">The telegram user id.</param>
	/// <param name="ct">Cancellation token</param>
	/// <returns>The user</returns>
	public async Task<User> GetAsyncId(Int64 userId, CancellationToken ct)
	{
		return await _users.Find(user => user.UserId == userId).FirstOrDefaultAsync(cancellationToken: ct);
	}

	/// <summary>
	/// Get a user by its document id.
	/// </summary>
	/// <param name="docId">The user's document id.</param>
	/// <param name="ct">Cancellation token</param>
	/// <returns></returns>
	public async Task<User> GetAsyncDocId(string docId, CancellationToken ct)
	{
		return await _users.Find(user => user.Id == docId).FirstOrDefaultAsync(cancellationToken: ct);
	}

	/// <summary>
	/// Create a user inside the database.
	/// </summary>
	/// <param name="user">The new user to insert in the database.</param>
	/// <param name="ct">Cancellation token</param>
	public async Task CreateAsync(User user, CancellationToken ct)
	{
		await _users.InsertOneAsync(user,cancellationToken:ct);
	}

	/// <summary>
	/// Update a user inside the database.
	/// </summary>
	/// <param name="ct">Cancellation token</param>
	/// <param name="user">The user to update</param>
	public async Task UpdateAsync(User user, CancellationToken ct)
	{
		await _users.ReplaceOneAsync(u => u.Id == user.Id, user, cancellationToken: ct);
	}

	/// <summary>
	/// Delete a user by it's document id.
	/// </summary>
	/// <param name="docId">The document id of the user.</param>
	/// <param name="ct">Cancellation token</param>
	public async Task DeleteAsync(string docId, CancellationToken ct)
	{
		await _users.DeleteOneAsync(u => u.Id == docId, cancellationToken: ct);
	}
}