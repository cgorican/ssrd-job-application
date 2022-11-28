# SSRD.io job application

.NET Core web application that reads data from the [website](https://feeds.meteoalarm.org/feeds/meteoalarm-legacy-atom-austria) and stores it in a SQLite database.  <br/>The project can be run within the Docker container.

Applications reads warnings and saves:
- region (areaDesc)
- onset (when the warning comes into effect)
- severity
- Author(name, url)

## Database Tables
- Warning
  - id: Int
  - region: String
  - onset: DateTime
  - severity: Int
  - author_id*
- Author
  - id: Int
  - name: String
  - url*: String

</details>

## Endpoints
### GET
- /Author
- /Author/:id
- /Parse
- /Warning
- /Warning/:severity

### POST
- /Author
- /Warning

### PUT
- /Author
- /Warning

### DELETE
- /Author/:id
- /Warning/:id
