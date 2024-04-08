import React, {useState, useEffect} from 'react';
import { Button } from "react-bootstrap";

function App() {
    
    const [state,setState] = useState([{}])
    
    useEffect(() => {
        GetSwitch();
        
        const interval = setInterval(() => {
            GetSwitch();
        }, 3000)
        
        return () => clearInterval(interval);
        
    }, [])
    
    function GetSwitch(){
        fetch("/api/switch").then(
            (response) => response.json()
        ).then(
            state => {
                setState(state)
                console.log(state)
            }
        )
    }
    
    function PostSwitch(newState) {
        fetch("/api/switch", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
            "id" : 1,
            "state" : newState
            })
        }).then(
            (response) => response.json()
        ).then(
            state => {
                setState(state)
                console.log(state)
                return state;
            }
        )
    }
    
    return (
        <div>
            {(typeof state.state === 'undefined') ? (
                <p>Loading...</p>
            ):(
                (state.state === 'true') ? (
                    <Button className="m-4" 
                            variant="success" 
                            onClick={()=>PostSwitch('false')}>
                        SWITCH
                    </Button>
                ) : (
                    <Button className="m-4" 
                            variant="danger" 
                            onClick={()=>PostSwitch('true')}>
                        SWITCH
                    </Button>
                )
            )
            }
        </div>
    );
}

export default App;