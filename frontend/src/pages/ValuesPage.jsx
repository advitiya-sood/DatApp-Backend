import { useEffect, useState } from 'react';
import { fetchValues } from '../services/apiClient.js';
import './ValuesPage.css';

export default function ValuesPage() {
  const [values, setValues] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    let cancelled = false;
    const load = async () => {
      try {
        const data = await fetchValues();
        if (!cancelled) {
          setValues(Array.isArray(data) ? data : []);
        }
      } catch (err) {
        if (!cancelled) {
          setError(err.message || 'Failed to load values');
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    };
    load();
    return () => {
      cancelled = true;
    };
  }, []);

  return (
    <div className="values-container">
      <section className="values-panel">
        <header className="values-header">
          <h1>Values</h1>
          <p>Data coming from the secured `api/values` endpoint.</p>
        </header>
        {loading && <div className="values-status">Loading values…</div>}
        {error && <div className="values-error">{error}</div>}
        {!loading && !error && (
          <ul className="values-list">
            {values.map((v) => {
              const id = v.id ?? v.Id;
              const name = v.name ?? v.Name;
              return (
                <li key={id} className="values-item">
                  <span className="values-pill">#{id}</span>
                  <span className="values-name">{name}</span>
                </li>
              );
            })}
            {values.length === 0 && (
              <li className="values-empty">No values found in the database.</li>
            )}
          </ul>
        )}
      </section>
    </div>
  );
}

