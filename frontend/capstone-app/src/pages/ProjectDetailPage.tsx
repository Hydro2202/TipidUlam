import React, { useEffect, useState } from 'react';
import { Container, Row, Col, Card, Button, Spinner, Alert, Table, Badge } from 'react-bootstrap';
import { useParams } from 'react-router-dom';
import { ProjectDetails, Task } from '../types';
import apiClient from '../services/apiClient';

const ProjectDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [project, setProject] = useState<ProjectDetails | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchProject = async () => {
      try {
        if (id) {
          const data = await apiClient.getProject(id);
          setProject(data);
        }
      } catch (err: any) {
        setError('Failed to load project details');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchProject();
  }, [id]);

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

  if (!project) {
    return (
      <Container className="py-5">
        <Alert variant="danger">Project not found</Alert>
      </Container>
    );
  }

  const getPriorityBadgeVariant = (priority: string) => {
    switch (priority.toLowerCase()) {
      case 'high':
        return 'danger';
      case 'medium':
        return 'warning';
      case 'low':
        return 'info';
      default:
        return 'secondary';
    }
  };

  const getStatusBadgeVariant = (status: string) => {
    switch (status.toLowerCase()) {
      case 'completed':
        return 'success';
      case 'in progress':
        return 'info';
      case 'not started':
        return 'secondary';
      default:
        return 'light';
    }
  };

  return (
    <Container className="py-5">
      <Row className="mb-4">
        <Col>
          <h1>{project.name}</h1>
          <p className="text-muted">{project.description || 'No description'}</p>
        </Col>
        <Col className="text-end">
          <Button variant="outline-primary">
            <i className="bi bi-pencil"></i> Edit
          </Button>
        </Col>
      </Row>

      {error && <Alert variant="danger">{error}</Alert>}

      <Row className="mb-4">
        <Col md={4}>
          <Card className="mb-3">
            <Card.Body>
              <Card.Text className="text-muted">Owner</Card.Text>
              <Card.Title>{project.ownerName || 'Unknown'}</Card.Title>
            </Card.Body>
          </Card>
        </Col>
        <Col md={4}>
          <Card className="mb-3">
            <Card.Body>
              <Card.Text className="text-muted">Total Tasks</Card.Text>
              <Card.Title>{project.tasks?.length || 0}</Card.Title>
            </Card.Body>
          </Card>
        </Col>
        <Col md={4}>
          <Card className="mb-3">
            <Card.Body>
              <Card.Text className="text-muted">Created</Card.Text>
              <Card.Title className="small">
                {new Date(project.createdAt).toLocaleDateString()}
              </Card.Title>
            </Card.Body>
          </Card>
        </Col>
      </Row>

      <Row>
        <Col>
          <Card>
            <Card.Header>
              <Card.Title className="mb-0">
                <i className="bi bi-list-task"></i> Tasks
              </Card.Title>
            </Card.Header>
            <Card.Body>
              {project.tasks && project.tasks.length > 0 ? (
                <Table responsive>
                  <thead>
                    <tr>
                      <th>Title</th>
                      <th>Status</th>
                      <th>Priority</th>
                      <th>Assigned To</th>
                    </tr>
                  </thead>
                  <tbody>
                    {project.tasks.map((task: Task) => (
                      <tr key={task.id}>
                        <td>{task.title}</td>
                        <td>
                          <Badge bg={getStatusBadgeVariant(task.status)}>
                            {task.status}
                          </Badge>
                        </td>
                        <td>
                          <Badge bg={getPriorityBadgeVariant(task.priority)}>
                            {task.priority}
                          </Badge>
                        </td>
                        <td>{task.assignedToName || 'Unassigned'}</td>
                      </tr>
                    ))}
                  </tbody>
                </Table>
              ) : (
                <p className="text-muted">No tasks yet</p>
              )}
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default ProjectDetailPage;
