import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Card, Button, Spinner, Alert } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { Project } from '../types';
import apiClient from '../services/apiClient';
import { useAuth } from '../services/authContext';

const ProjectsPage: React.FC = () => {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const { user } = useAuth();

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        if (user) {
          const data = await apiClient.getUserProjects(user.id);
          setProjects(data);
        }
      } catch (err: any) {
        setError('Failed to load projects');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchProjects();
  }, [user]);

  if (loading) {
    return (
      <Container className="py-5">
        <div className="text-center">
          <Spinner animation="border" role="status">
            <span className="visually-hidden">Loading...</span>
          </Spinner>
        </div>
      </Container>
    );
  }

  return (
    <Container className="py-5">
      <Row className="mb-4">
        <Col>
          <h1>My Projects</h1>
        </Col>
        <Col className="text-end">
          <Button variant="success">
            <i className="bi bi-plus-circle"></i> New Project
          </Button>
        </Col>
      </Row>

      {error && <Alert variant="danger">{error}</Alert>}

      {projects.length === 0 ? (
        <Row>
          <Col md={8} className="mx-auto">
            <Card className="text-center">
              <Card.Body className="py-5">
                <Card.Title>No projects yet</Card.Title>
                <Card.Text>Create your first project to get started</Card.Text>
                <Button variant="primary">Create Project</Button>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      ) : (
        <Row>
          {projects.map((project) => (
            <Col md={6} lg={4} key={project.id} className="mb-4">
              <Card className="h-100 shadow-sm">
                <Card.Body>
                  <Card.Title className="text-truncate">{project.name}</Card.Title>
                  <Card.Text className="text-muted small">
                    {project.description || 'No description'}
                  </Card.Text>
                  <Card.Text className="text-muted small">
                    <strong>Owner:</strong> {project.ownerName || 'Unknown'}
                  </Card.Text>
                </Card.Body>
                <Card.Footer className="bg-transparent">
                  <Link to={`/projects/${project.id}`} className="text-decoration-none">
                    <Button variant="outline-primary" size="sm" className="w-100">
                      View Details
                    </Button>
                  </Link>
                </Card.Footer>
              </Card>
            </Col>
          ))}
        </Row>
      )}
    </Container>
  );
};

export default ProjectsPage;
