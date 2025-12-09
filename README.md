# HealthCalendar

A healthcare appointment management system with separate interfaces for patients, healthcare workers, and administrators. Project is split into a backend (/api) and a frontend (/healthcalendar).

### Setup requirements
Node.js: 20.x (recommended stable LTS version) <br>
npm: 10.x or higher

### Running the project
```bash
cd healthcalendar
npm install
npm run dev
```
The project now runs at `http://localhost:5173`

### Existing login information

| Role    | E-mail            | Password |
|--------|------------------|----------|
| Patient | lars@gmail.com   | Aaaa4@   |
| Worker  | bong@gmail.com   | Aaaa4@   |
| Admin   | baifan@gmail.com | Aaaa4@   |

### Testing
```bash
cd api.Tests
dotnet test
```

### Functionality
**Patients** can book/create events in a time period their healthcare worker has set themselves to available. The patient can also see, update and delete their booked events in a calendar view (full CRUD operations). <br>

**Workers** can view events their patients have booked in a calendar view. A worker can click the 'change availability' button, toggling their availability. If the worker clicks the 'repeat weekly' toggle, the availability will repeat forever weekly. Clicking on an event gives the worker more details. This fulfills CRUD for worker (create, view, update, and delete availability schedules).

**Admins** can view all users in the system, manage patients-worker relationships, and delete users. Admin can also register healthcare workers.
