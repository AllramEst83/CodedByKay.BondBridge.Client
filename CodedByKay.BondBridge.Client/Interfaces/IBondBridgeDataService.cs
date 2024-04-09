using CodedByKay.BondBridge.Client.Models.Request;
using CodedByKay.BondBridge.Client.Models.Response;

namespace CodedByKay.BondBridge.Client.Interfaces
{
    /// <summary>
    /// Interface for retrieving data from a bond bridge.
    /// </summary>
    public interface IBondBridgeDataService
    {
        /// <summary>
        /// Retrieves data of specified type from the specified URI.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <param name="uri">The URI from which to retrieve the data.</param>
        /// <returns>The retrieved data as an object of the specified type.</returns>
        Task<T> GetData<T>(string uri) where T : class;
        Task<CreateGroupResponse> CreateGroup(CreateGroupRequest requestData, string uri);
        Task<bool> Delete(string uri, Guid id);
    }
}
