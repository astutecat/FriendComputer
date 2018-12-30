using Microsoft.EntityFrameworkCore.Migrations;

namespace Carl.Migrations
{
  public partial class InitialCreate : Migration
  {
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "Quotes");
    }

    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(name: "Quotes",
        columns: table => new
        {
          Id = table.Column<int>(nullable: false)
          .Annotation("Sqlite:Autoincrement", true),
          Channel = table.Column<string>(nullable: false),
          Author = table.Column<string>(nullable: false),
          Text = table.Column<string>(nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Quotes", x => x.Id);
        });
    }
  }
}
