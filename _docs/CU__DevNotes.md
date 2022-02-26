# Contoso University - Dev Notes

<table>
    <tr>
        <th>Date</th><th>Dev</th>
		<th>Notes</th>
    </tr>
    <tr>
        <td>2/26/2022</td><td>bg</td>
		<td>
            Added Course Delete, updated content for Home page<br/>
            Added logging with NLog<br/>
            Added Blazor demo components<br/>
		</td>
    </tr>
    <tr>
        <td>2/25/2022</td><td>bg</td>
		<td>
            Added SchoolViewDataRepository, Course Details<br/>
            Using Ardalis.GuardClauses<br/>
            Added Create Course<br/>
            Added Course Edit<br/>
		</td>
    </tr>
    <tr>
        <td>2/24/2022</td><td>bg</td>
		<td>
            Restarted from scratch - Models, Common and DAL projects<br/>
            Added initial persistent Models<br/>
            Initial EF Migration - 
            see _MigrationNotes in ContosoUniversity.DAL<br/>
            Tweaked Schema1.sql to make __MigrationHistory optional<br/>
            Added ASP.NET Core 3.1 MVC web app - ContosoUniversity<br/>
            Added new project ContosoUniversity.Shared for shared interfaces, ViewModels, etc.<br/>
            Added SchoolRepository<br/>
            Added CoursesController with Index page<br/>
            Completed switch to attribute routing<br/>
            Added Departments controller with Index page<br/>
            Added Instructors and Students controllers w/Index pages<br/>
            Added ISchoolDbContext and data seeding<br/>
            Started work on paging students<br/>
		</td>
    </tr>
    <tr>
        <td></td><td></td>
		<td>
		</td>
    </tr>
</table>
