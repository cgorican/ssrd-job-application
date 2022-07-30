# Task for job application at SSRD.io

.NET Core web application that reads data from the [website](https://feeds.meteoalarm.org/feeds/meteoalarm-legacy-atom-austria) and stores it in a SQLite database.
The project can be run within the Docker container.

Applications reads warnings and saves:
- region (areaDesc)
- onset (when the warning comes into effect)
- severity
- name of the author

## Database Tables
<details><summary>Warning</summary>

- id
- areaDesc
- onset
- severity
- author_id

</details>

<details><summary>Author</summary>

- id
- name
- url*

</details>

## Paths
<details><summary>GET</summary>

- Author
- Author/:id
- Parse
- Warning
- Warning/:severity

</details>

<details><summary>POST</summary>

- Author
- Warning

</details>

<details><summary>PUT</summary>

- Author
- Warning

</details>

<details><summary>DELETE</summary>

- Author/:id
- Warning/:id

</details>