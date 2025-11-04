namespace UserMasterCategory.API.Models;

public class UserMaster  // ✅ Class name CHANGE to UserMaster
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string phone_number { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public bool is_active { get; set; } = true;
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
}