using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solster.AspNetCore.ActivityLog.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ActivityLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                Category = table.Column<short>(type: "smallint", nullable: false),
                Action = table.Column<short>(type: "smallint", nullable: false),
                ResourceType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                ResourceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                ResourceLabel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                Actor = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                ActorIp = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                CorrelationId = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActivityLogs", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ActivityLogs_Actor_Timestamp",
            table: "ActivityLogs",
            columns: ["Actor", "Timestamp"]);

        migrationBuilder.CreateIndex(
            name: "IX_ActivityLogs_ResourceType_ResourceId_Timestamp",
            table: "ActivityLogs",
            columns: ["ResourceType", "ResourceId", "Timestamp"]);

        migrationBuilder.CreateIndex(
            name: "IX_ActivityLogs_Timestamp",
            table: "ActivityLogs",
            column: "Timestamp");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ActivityLogs");
    }
}
