import './App.css';
import { BrowserRouter as Router, Route, Switch} from "react-router-dom";
import Login from "./Pages/Login";
import Register from "./Pages/Register";
import Home from "./Pages/Home";
import Posts from "./Pages/Posts";


function App() {
  return (
    <Router>
      <Switch>
        <Route exact path="/login" component={Login} />
        <Route exact path="/register" component={Register} />
        <Route exact path="/home" component={Home}/>
        <Route exact path="/post/:id" component={Posts}/>
      </Switch>
    </Router>
  );
}

export default App;
