import React from "react";
import "../App.css";
import Logo from "../img/Yomu_Logo.png";
import { useHistory } from "react-router-dom";
import { useState } from "react";
import SwaggerClient from 'swagger-client';

function Register(){

    const[username, setUsername] = useState("");
    const[email, setEmail] = useState("");
    const[password, setPassword] = useState("");
    const[repeatpassword, setRepeatpassword] = useState("");


    const handleSubmit = (event) => {
        event.preventDefault();
        if(password === repeatpassword){
            new SwaggerClient('http://localhost/swagger/v1/swagger.json')
            .then(
                client => client.apis.default.postLogin(email, password),
                reason => console.error('failed to load the spec: ' + reason)
              )
              .then(
                addPetResult => console.log(addPetResult.body),
                reason => console.error('failed on api call: ' + reason)
              );
        }
    }

    let history = useHistory();
    return(
        <div className="Login-form">
            <img src={Logo} alt="Yomu logo" className="logo"></img><br></br>
            <h2>Registrierung</h2>
            <div className="Login-forminput">
                <form onSubmit={handleSubmit}>
                    <input value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    type="text" placeholder="Benutzername" className="username"/><br></br>
                    <input value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    type="email" placeholder="Email" className="email"/><br></br>
                    <input value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    type="password" placeholder="Passwort" className="password"/><br></br>
                    <input value={repeatpassword}
                    onChange={(e) => setRepeatpassword(e.target.value)}
                    type="password" placeholder="Passwort wiederholen" className="repeatpassword"/><br></br>
                    <p >Passwörter stimmen nicht überein!</p>
                    <input type="submit" className="Login-formbutton"/><br></br>
                    Schon Mitglied?<button className="Login-formbutton"
                    onClick={() => {
                     history.push("/Login")
                 }}>Login</button>
                </form>
            </div>
        </div>
    )
}


export default Register;