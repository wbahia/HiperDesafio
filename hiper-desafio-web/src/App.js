import React, { useState, useEffect } from 'react';
import axios from 'axios';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  const [pedidos, setPedidos] = useState([]);
  const [form, setForm] = useState({ descricao: '', valor: 0, tipoCliente: 'Comum' });
  const api = axios.create({ baseURL: 'http://localhost:5033/api' });

  // Carregar pedidos
  const fetchPedidos = async () => {
    const res = await api.get('/pedidos');
    setPedidos(res.data);
  };

  useEffect(() => { fetchPedidos(); }, []);

  // Enviar pedido
  const handleSubmit = async (e) => {
    e.preventDefault();
    await api.post('/pedidos', form);
    setForm({ descricao: '', valor: 0, tipoCliente: 'Comum' });
    fetchPedidos(); // Atualiza a lista
  };

  return (
    <div className="container mt-5">
      <h1>Hiper Pedidos 🛒</h1>
      
      <form onSubmit={handleSubmit} className="card p-4 mb-5 shadow">
        <div className="mb-3">
          <label className="form-label">Descrição</label>
          <input type="text" className="form-control" value={form.descricao} 
            onChange={e => setForm({...form, descricao: e.target.value})} />
        </div>
        <div className="mb-3">
          <label className="form-label">Valor Original</label>
          <input type="number" className="form-control" value={form.valor} 
            onChange={e => setForm({...form, valor: parseFloat(e.target.value)})} />
        </div>
        <div className="mb-3">
          <label className="form-label">Tipo de Cliente</label>
          <select className="form-select" value={form.tipoCliente} 
            onChange={e => setForm({...form, tipoCliente: e.target.value})}>
            <option value="Comum">Comum (5% Desconto)</option>
            <option value="VIP">VIP (15% Desconto)</option>
          </select>
        </div>
        <button type="submit" className="btn btn-primary">Criar Pedido</button>
      </form>

      <h3>Lista de Pedidos</h3>
      <table className="table table-striped border">
        <thead>
          <tr>
            <th>Descrição</th>
            <th>Original</th>
            <th>Com Desconto</th>
            <th>Tipo</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {pedidos.map(p => (
            <tr key={p.id}>
              <td>{p.descricao}</td>
              <td>R$ {p.valor}</td>
              <td className="text-success fw-bold">R$ {p.valorFinal}</td>
              <td>{p.tipoCliente}</td>
              <td><span className="badge bg-warning text-dark">{p.status}</span></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default App;