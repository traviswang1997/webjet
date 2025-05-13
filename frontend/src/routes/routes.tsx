import { Route, Switch } from "wouter";
import MovieListPage from "../pages/MovieListPage";
import MovieDetailsPage from "../pages/MovieDetailsPage";
//import CompareMoviesPage from './pages/CompareMoviesPage';

const Routes = () => {
  return (
    <Switch>
      <Route path="/" component={MovieListPage} />
      <Route path="/:providerId/movies/:movieId" component={MovieDetailsPage} />
      <Route path="/movies/:title" component={MovieDetailsPage} />
      {/* <Route path="/compare" component={CompareMoviesPage} /> */}
    </Switch>
  );
};

export default Routes;
