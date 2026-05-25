import React from 'react';
import { Container, Row, Col, Card, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import { useAuth } from '../services/authContext';

const DashboardPage: React.FC = () => {
  const { isAuthenticated, user } = useAuth();

  return (
    <Container className="py-5">
      <Row className="mb-4">
        <Col>
          <h1 className="mb-4">Project Management Dashboard</h1>
        </Col>
      </Row>

      {isAuthenticated ? (
        <Row>
          <Col md={6} className="mb-4">
            <Card className="h-100">
              <Card.Body>
                <Card.Title>
                  <i className="bi bi-person-circle"></i> Welcome, {user?.name}!
                </Card.Title>
                <Card.Text>
                  Manage your projects efficiently and track your tasks.
                </Card.Text>
                <Link to="/projects">
                  <Button variant="primary">
                    <i className="bi bi-kanban"></i> View Projects
                  </Button>
                </Link>
              </Card.Body>
            </Card>
          </Col>

          <Col md={6} className="mb-4">
            <Card className="h-100">
              <Card.Body>
                <Card.Title>
                  <i className="bi bi-graph-up"></i> Quick Stats
                </Card.Title>
                <Card.Text className="mb-3">
                  <strong>Email:</strong> {user?.email}
                </Card.Text>
                <Card.Text>
                  <strong>Member Since:</strong> {new Date(user?.createdAt || '').toLocaleDateString()}
                </Card.Text>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      ) : (
        <Row>
          <Col md={8} className="mx-auto">
            <Card className="text-center">
              <Card.Body className="py-5">
                <Card.Title className="mb-4">Welcome to Capstone</Card.Title>
                <Card.Text className="mb-4">
                  A professional project management platform to help you organize and track your work.
                </Card.Text>
                <Link to="/login">
                  <Button variant="primary" size="lg">
                    Get Started - Login
                  </Button>
                </Link>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      )}
    </Container>
  );
};

export default DashboardPage;
