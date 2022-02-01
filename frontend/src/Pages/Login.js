import React from "react";
import "../App.css";
import Logo from "../img/Yomu_Logo.png";
import { useHistory } from "react-router-dom";
import SwaggerClient from 'swagger-client';

function Login() {

    let history = useHistory();

    return(
        <div className="Login-form">
            <img src={Logo} alt="Yomu logo" className="logo"></img><br></br>
            <h2>Login</h2>
            <div className="Login-forminput">
                <input type="text" placeholder="Benutzername" className="username"/> <br></br>
                <input type="password" placeholder="Passwort" className="password"/><br></br>
                <button className="Login-formbutton"> Login </button><br></br>
                Noch kein Account?<button className="Login-formbutton"
                onClick={() => {
                    history.push("/Register")
                }}> 
                Registrieren</button>
            </div>
        </div>
    )
}

export default Login;