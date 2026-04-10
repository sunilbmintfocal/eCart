import { useRoutes } from 'react-router-dom';
import routes from './routes';
import { UIProvider } from './context/UIContext';

function App() {
  const element = useRoutes(routes);

  return (
    <UIProvider>
      {element}
    </UIProvider>
  );
}

export default App;
