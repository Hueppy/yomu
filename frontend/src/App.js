import './App.css';
import { BrowserRouter as Router, Route, Switch} from "react-router-dom";
import Login from "./Pages/Login";
import Register from "./Pages/Register";
import Home from "./Pages/Home";
import Posts from "./Pages/Posts";
import Communities from "./Pages/Communities";
import { Buffer } from 'buffer';

function App() {
  window.Buffer = Buffer;
  return (
    <Router>
      <Switch>
        <Route exact path="/login" component={Login} />
        <Route exact path="/register" component={Register} />
        <Route exact path="/home" component={Home}/>
        <Route exact path="/post/:id" component={Posts}/>
        <Route exact path="/community/:id" component={Communities}/>
      </Switch>
    </Router>
  );
}

export default App;
