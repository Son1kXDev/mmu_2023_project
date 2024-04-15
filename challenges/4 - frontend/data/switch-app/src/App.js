import React, {useState, useEffect} from 'react';

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
                console.log("GET: " + state.state)
            }
        );
    }
    
    function PostSwitch(newState) {
        fetch("/api/switch", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
            "id" : 1,
            "state" : newState.toLocaleString()
            })
        }).then(
            (response) => response.json()
        ).then(
            state => {
                setState(state)
                console.log("POST: " + state.state)
                return state;
            }
        );
    }
    
    return (
        <div>
            {(typeof(state.state) === 'undefined') ? (
                <p>Loading...</p>
            ): (
                <button id="switch-button" 
                        className={((state.state === 'true') ? "enabled" : "disabled") } 
                        onClick={ () => 
                            PostSwitch((state.state === 'true') ? 'false' : 'true')
                }>
                    SWITCH
                </button>
            )}
        </div>
    );
    
}

export default App;