import './App.css';
import { AuthProvider } from './context/AuthContext';
import Root from './routes/Root';

function App() {

  return (
    <div className="App">
      <AuthProvider>
        <Root />
      </AuthProvider>
    </div>
  );
}

export default App;
