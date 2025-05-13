# MovieCompare – Cinema Price Comparison App

## Tech Stack

### Frontend:
- React 18 + TypeScript
- Zustand (global state management - todo if client side data need to be considered)
- React Query (data fetching + caching)
- TailwindCSS (UI styling)
- Wouter (routing)
- React Toastify (notifications - todo)

### Backend:
- .NET 8 Web API
- RESTful endpoints
- In-memory caching via IMemoryCache
- NSwag-generated TypeScript client
- XUnit + Moq (unit tests)

---

### System Design
![image](https://github.com/user-attachments/assets/5ba9fd4b-6d05-446e-91f1-1d1d44f4fe38)

---

### Functional
- Search by movie title
- Filter by cinema
- Paginated API
- **Compare ticket prices**
- Smart deduplication across providers
- (Toasts for user feedback)
- Graceful fallback if image fails to load

### Non-Functional
- caching (5 min window)
- Debounced interactions
- Dockerized (frontend + backend)
- Unit test coverage on API (90%)
- Secure API keys (via environment variables)
- ESLint + Prettier enforced formatting

---

## TODO

- [ ] Add integration tests for BFF API
- [ ] Add provider health-check UI indicator
- [ ] Support server-sent events or polling for real-time price updates
- [ ] Database for persistency

---

## How to start

### Backend
 cd backend \
 dotnet restore \
 dotnet build \
 dotnet run

### Frontend
 cd frontend \
 npm install \
 npm run dev

### Docker
- docker-compose up --build
